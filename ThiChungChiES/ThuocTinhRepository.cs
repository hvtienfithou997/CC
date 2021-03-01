using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiChungChiModels;

namespace ThiChungChiES
{
    public class ThuocTinhRepository : IESRepository
    {
        #region Init

        protected static string _default_index;

        //protected new ElasticClient client;
        private static ThuocTinhRepository instance;

        public ThuocTinhRepository(string modify_index)
        {
            _default_index = !string.IsNullOrEmpty(modify_index) ? modify_index : _default_index;
            ConnectionSettings settings = new ConnectionSettings(connectionPool, sourceSerializer: Nest.JsonNetSerializer.JsonNetSerializer.Default).DefaultIndex(_default_index).DisableDirectStreaming(true);
            settings.MaximumRetries(10);
            client = new ElasticClient(settings);
            var ping = client.Ping(p => p.Pretty(true));
            if (ping.ServerError != null && ping.ServerError.Error != null)
            {
                throw new Exception("START ES FIRST");
            }
        }

        public static ThuocTinhRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    _default_index = "cc_thuoc_tinh";
                    instance = new ThuocTinhRepository(_default_index);
                }
                return instance;
            }
        }

        public bool CreateIndex(bool delete_if_exist = false)
        {
            if (delete_if_exist && client.Indices.Exists(_default_index).Exists)
                client.Indices.Delete(_default_index);

            var createIndexResponse = client.Indices.Create(_default_index, s => s.Settings(st => st.NumberOfReplicas(0).NumberOfShards(3)).Map<ThuocTinh>(mm => mm.AutoMap().Dynamic(true)));
            return createIndexResponse.IsValid;
        }

        #endregion Init

        public IEnumerable<ThuocTinh> GetAll(string app_id, int page_index, int page_size)
        {
            var re = client.Search<ThuocTinh>(s => s.Query(q => q.Term(t => t.Field("app_id.keyword").Value(app_id))).Size(page_size).From(page_size * (page_index - 1)));
            List<ThuocTinh> lc = new List<ThuocTinh>();
            foreach (var item in re.Hits)
            {
                lc.Add(ConvertDoc(item));
            }

            return lc;
        }

        public bool Index(ThuocTinh data, out string msg)
        {
            bool need_retry = true;
            var gia_tri_random = Nanoid.Nanoid.Generate(alphabet: "1234567890", size: 8);
            msg = "";
            var max_gia_tri = Convert.ToInt32(gia_tri_random);
            if (!string.IsNullOrEmpty(data.ten))
            {
                if (!IsGiaTriThuocTinhExist(data.loai, data.type, data.gia_tri))
                {
                    data.gia_tri = max_gia_tri;
                }
                int retry = 0; int max_retry = 5;

                while (retry++ < max_retry && need_retry)
                {
                    need_retry = !Index(_default_index, data, "");
                    if (need_retry)
                        Task.Delay(1000).Wait();
                }
            }
            msg = "Nhập tên thuộc tính";
            return !need_retry;
        }

        public bool Update(ThuocTinh data)
        {
            if (!string.IsNullOrEmpty(data.ten))
            {
                string id = $"{data.id}";
                data.id = string.Empty;
                data.ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
                return Update(_default_index, data, id);
            }

            return false;
        }

        public bool Delete(string id)
        {
            return DeleteById<ThuocTinh>(id);
        }

        private ThuocTinh ConvertDoc(IHit<ThuocTinh> hit)
        {
            ThuocTinh u = new ThuocTinh();

            try
            {
                u = hit.Source;
                u.id = hit.Id;
            }
            catch
            {
            }
            return u;
        }

        public List<ThuocTinh> Search(string term, int loai, string nguoi_tao, int page, out long total_recs,
            out string msg, int page_size, bool is_admin = false)
        {
            msg = "";
            total_recs = 0;
            List<ThuocTinh> lst = new List<ThuocTinh>();
            try
            {
                List<QueryContainer> must = new List<QueryContainer>();
                List<QueryContainer> must_not = new List<QueryContainer>
                {
                    new TermQuery()
                    {
                        Field = "trang_thai",
                        Value = TrangThai.DELETED
                    }
                };
                if (!string.IsNullOrEmpty(term))
                {
                    must.Add(new QueryStringQuery() { Fields = new string[] { "ten" }, Query = term });
                }
                if (loai > -1)
                {
                    must.Add(new TermQuery()
                    {
                        Field = "loai",
                        Value = loai
                    });
                }
                if (!is_admin)
                {
                    if (!string.IsNullOrEmpty(nguoi_tao))
                    {
                        must.Add(new TermQuery()
                        {
                            Field = "nguoi_tao.keyword",
                            Value = nguoi_tao
                        });
                    }
                }
                List<ISort> sort = new List<ISort>
                {
                    new FieldSort() {Field = "ngay_sua", Order = SortOrder.Descending},
                    new FieldSort() {Field = "ngay_tao", Order = SortOrder.Descending}
                };
                SearchRequest req = new SearchRequest(_default_index);
                req.Query = new QueryContainer(new BoolQuery() { Must = must, MustNot = must_not });
                req.From = (page - 1) * page_size;
                req.Size = page_size;
                req.Sort = sort;
                req.TrackTotalHits = true;
                var res = client.Search<ThuocTinh>(req);
                if (res.IsValid)
                {
                    total_recs = res.Total;
                    lst = res.Hits.Select(ConvertDoc).ToList();
                }
            }
            catch (Exception ex)
            {
                msg = $"{ex.Message}. Trace: {ex.StackTrace}";
            }
            return lst;
        }

        public ThuocTinh GetById(string id)
        {
            var obj = GetById<ThuocTinh>(_default_index, id);
            if (obj != null)
            {
                obj.id = id;
                return obj;
            }
            return null;
        }

        public bool IsGiaTriThuocTinhExist(LoaiThuocTinh loai, ThuocTinhType type, int gia_tri)
        {
            var re = client.Search<ThuocTinh>(s => s
                .Query(q => q.Term(t => t.Field("trang_thai").Value(TrangThai.ACTIVE)) &&
                            q.Term(t => t.Field("gia_tri").Value(gia_tri))
            ).Size(0));
            if (re.IsValid)
                return re.Total > 0;
            return true;
        }

        public List<ThuocTinh> GetThuocTinhByLoai(LoaiThuocTinh loai, string user, bool is_admin = false)
        {
            if (is_admin)
            {
                var re = client.Search<ThuocTinh>(s => s
                    .Source(so => so.Includes(ic => ic.Fields(new string[] { "loai", "gia_tri", "nhom", "type", "ten" })))
                    .Query(q => q.Term(t => t.Field("loai").Value(loai))
                    ).Size(100));
                return re.Hits.Select(ConvertDoc).ToList();
            }
            else
            {
                var re = client.Search<ThuocTinh>(s =>
                    s.Source(
                            so => so.Includes(ic => ic.Fields(new string[] {"loai", "gia_tri", "nhom", "type", "ten"})))
                        .Query(q => q.Term(t => t.Field("loai").Value(loai)) &&
                                    q.Term(t => t.Field("nguoi_tao").Value(user))).Size(100));
                return re.Hits.Select(ConvertDoc).ToList();
            }
        }

        public List<ThuocTinh> GetManyByGiaTri(IEnumerable<int> lst_gia_tri, LoaiThuocTinh loai, bool is_admin = false)
        {
            List<ThuocTinh> lst = new List<ThuocTinh>();

            if (lst_gia_tri.Count() > 0)
            {
                var re = client.Search<ThuocTinh>(s => s.Query(q =>
                    q.Term(t => t.Field("loai").Value(loai)) && q.Terms(t => t.Field("gia_tri").Terms(lst_gia_tri))));
                return re.Hits.Select(x => ConvertDoc(x)).ToList();
            }
            return lst;
        }
    }
}
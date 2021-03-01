using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiChungChiModels;

namespace ThiChungChiES
{
    public class DanhSachThiRepository : IESRepository
    {
        #region Init

        protected static string _default_index;

        //protected new ElasticClient client;
        private static DanhSachThiRepository instance;

        public DanhSachThiRepository(string modify_index)
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

        public static DanhSachThiRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    _default_index = "cc_danh_sach_thi";
                    instance = new DanhSachThiRepository(_default_index);
                }
                return instance;
            }
        }

        public bool CreateIndex(bool delete_if_exist = false)
        {
            if (delete_if_exist && client.Indices.Exists(_default_index).Exists)
                client.Indices.Delete(_default_index);

            var createIndexResponse = client.Indices.Create(_default_index, s => s.Settings(st => st.NumberOfReplicas(0).NumberOfShards(3)).Map<DanhSachThi>(mm => mm.AutoMap().Dynamic(true)));
            return createIndexResponse.IsValid;
        }

        #endregion Init

        public bool Index(DanhSachThi data)
        {
            int retry = 0; int max_retry = 5;
            bool need_retry = true;
            while (retry++ < max_retry && need_retry)
            {
                need_retry = !Index(_default_index, data, "");
                if (need_retry)
                    Task.Delay(1000).Wait();
            }
            return !need_retry;
        }

        public bool Update(DanhSachThi data)
        {
            string id = $"{data.id}";
            data.id = string.Empty;
            data.ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);

            return Update(_default_index, data, id);
        }

        public bool Delete(string id)
        {
            return DeleteById<DanhSachThi>(id);
        }

        private DanhSachThi ConvertDoc(IHit<DanhSachThi> hit)
        {
            DanhSachThi u = new DanhSachThi();

            try
            {
                u = hit.Source;
                u.id = hit.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return u;
        }

        public List<DanhSachThi> GetAll(List<string> ids_ignore, string[] view_fields)
        {
            List<QueryContainer> must_not = new List<QueryContainer>();
            if (ids_ignore.Count > 0)
            {
                must_not.Add(new IdsQuery()
                {
                    Values = ids_ignore.Select(x => (Id)x)
                }); ;
            }
            var query = new QueryContainer(new BoolQuery() { MustNot = must_not });
            var source = new SourceFilter() { Includes = view_fields };
            var all = GetObjectScroll<DanhSachThi>(_default_index, query, source);
            return all.Select(HitToDocument).ToList();
        }

        public List<DanhSachThi> GetAll(List<string> ids_ignore, string[] view_fields, string user, bool is_admin = false)
        {
            List<QueryContainer> must_not = new List<QueryContainer>();
            List<QueryContainer> must = new List<QueryContainer>();
            if (ids_ignore.Count > 0)
            {
                must_not.Add(new IdsQuery()
                {
                    Values = ids_ignore.Select(x => (Id)x)
                }); ;
            }

            if (!is_admin)
            {
                if (!string.IsNullOrEmpty(user))
                {
                    must.Add(new TermQuery()
                    {
                        Field = "id_tai_khoan.keyword",
                        Value = user
                    });
                }
            }
            var query = new QueryContainer(new BoolQuery() { MustNot = must_not, Must = must });
            var source = new SourceFilter() { Includes = view_fields };
            var all = GetObjectScroll<DanhSachThi>(_default_index, query, source);
            return all.Select(HitToDocument).ToList();
        }

        public List<DanhSachThi> Search(string term, string id_cuoc_thi, long ngay_bd, long ngay_kt, string nguoi_tao, IEnumerable<int> thuoc_tinh, int page, out long total_recs, out string msg, int page_size = 50, bool is_admin = false)
        {
            msg = "";
            total_recs = 0;
            List<DanhSachThi> lst = new List<DanhSachThi>();
            try
            {
                List<QueryContainer> must = new List<QueryContainer>();
                List<QueryContainer> should = new List<QueryContainer>();
                List<QueryContainer> must_not = new List<QueryContainer>();

                if (!string.IsNullOrEmpty(term))
                {
                    must.Add(new QueryStringQuery() { Fields = new string[] { "id_cuoc_thi", "id_tai_khoan" }, Query = term });
                }

                if (!string.IsNullOrEmpty(id_cuoc_thi))
                {
                    must.Add(new TermQuery()
                    {
                        Field = "id_cuoc_thi.keyword",
                        Value = id_cuoc_thi
                    });
                }
                if (thuoc_tinh.Count() > 0)
                {
                    must.Add(new TermsQuery()
                    {
                        Field = "thuoc_tinh",
                        Terms = thuoc_tinh.Select(x => (object)x)
                    });
                }

                if (!is_admin)
                {
                    if (!string.IsNullOrEmpty(nguoi_tao))
                    {
                        must.Add(new TermQuery()
                        {
                            Field = "id_tai_khoan.keyword",
                            Value = nguoi_tao
                        });
                    }
                }

                var cuoc_thi_period = CuocThiRepository.Instance.SearchByPeriod(ngay_bd, ngay_kt, is_admin).Select(x => x.id); ;

                if (cuoc_thi_period.Count() > 0)
                {
                    must.Add(new TermsQuery()
                    {
                        Field = "id_cuoc_thi.keyword",
                        Terms = cuoc_thi_period
                    });
                }
                else
                {
                    must.Add(new TermQuery()
                    {
                        Field = "id_cuoc_thi.keyword",
                        Value = cuoc_thi_period.Select(x => (object)x)
                    });
                }
                List<ISort> sort = new List<ISort>
                {
                    new FieldSort() {Field = "ngay_tao", Order = SortOrder.Descending},
                };
                SearchRequest req = new SearchRequest(_default_index);
                req.Query = new QueryContainer(new BoolQuery() { Must = must, MustNot = must_not, Should = should });
                req.From = (page - 1) * page_size;
                req.Size = page_size;
                req.Sort = sort;
                req.TrackTotalHits = true;
                var res = client.Search<DanhSachThi>(req);
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

        public DanhSachThi GetById(string id)
        {
            var obj = GetById<DanhSachThi>(_default_index, id);
            if (obj != null)
            {
                obj.id = id;
                return obj;
            }
            return null;
        }

        public bool UpdateThuocTinh(string id, List<int> thuoc_tinh)
        {
            return UpdateThuocTinh<CuocThi>(id, thuoc_tinh);
        }

        public List<DanhSachThi> GetDanhSachThiByIdCuocThi(string id)
        {
            var re = client.Search<DanhSachThi>(s => s.Query(
                q => q.Term(t => t.Field("id_cuoc_thi.keyword").Value(id))
            ));
            return re.Hits.Select(x => ConvertDoc(x)).ToList();
        }

        public DanhSachThi GetDanhSachThiByIdCuocThi(string id, string tai_khoan)
        {
            var re = client.Search<DanhSachThi>(s => s.Query(
                q => q.Term(t => t.Field("id_cuoc_thi.keyword").Value(id)) && q.Term(t => t.Field("id_tai_khoan.keyword").Value(tai_khoan))
            ));
            if (re.Total > 0)
            {
                var danh_sach = re.Hits.Select(x => ConvertDoc(x)).First();
                return danh_sach;
            }

            return null;
        }

        public List<DanhSachThi> GetDanhSachThiByIdTaiKhoan(string id)
        {
            var re = client.Search<DanhSachThi>(s => s.Query(
                q => q.Term(t => t.Field("id_tai_khoan.keyword").Value(id))
            ));
            return re.Hits.Select(x => ConvertDoc(x)).ToList();
        }

        public bool dele()
        {
            var re = client.DeleteByQuery<DanhSachThi>(q => q.Index(_default_index).MatchAll());
            return false;
        }
    }
}
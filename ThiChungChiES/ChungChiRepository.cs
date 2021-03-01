using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiChungChiModels;

namespace ThiChungChiES
{
    public class ChungChiRepository : IESRepository
    {
        #region Init

        protected static string _default_index;

        //protected new ElasticClient client;
        private static ChungChiRepository instance;

        public ChungChiRepository(string modify_index)
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

        public static ChungChiRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    _default_index = "cc_chung_chi";
                    instance = new ChungChiRepository(_default_index);
                }
                return instance;
            }
        }

        public bool CreateIndex(bool delete_if_exist = false)
        {
            if (delete_if_exist && client.Indices.Exists(_default_index).Exists)
                client.Indices.Delete(_default_index);
            var createIndexResponse = client.Indices.Create(_default_index, s => s.Settings(st => st.NumberOfReplicas(0).NumberOfShards(3)).Map<ChungChi>(mm => mm.AutoMap().Dynamic(true)));
            return createIndexResponse.IsValid;
        }

        #endregion Init

        public bool Index(ChungChi data)
        {
            int retry = 0; int max_retry = 5;
            bool need_retry = true;
            if (!string.IsNullOrEmpty(data.ten))
            {
                while (retry++ < max_retry && need_retry)
                {
                    need_retry = !Index(_default_index, data, "");
                    if (need_retry)
                        Task.Delay(1000).Wait();
                }
            }
            return !need_retry;
        }

        public bool Update(ChungChi data)
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
            return DeleteById<ChungChi>(id);
        }

        private ChungChi ConvertDoc(IHit<ChungChi> hit)
        {
            ChungChi u = new ChungChi();

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

        public List<ChungChi> Search(string term, string nguoi_tao, IEnumerable<int> thuoc_tinh, int page, out long total_recs, out string msg, int page_size = 50, bool is_admin = false)
        {
            msg = "";
            total_recs = 0;
            List<ChungChi> lst = new List<ChungChi>();
            try
            {
                List<QueryContainer> must = new List<QueryContainer>();
                List<QueryContainer> must_not = new List<QueryContainer>();
                if (!string.IsNullOrEmpty(term))
                {
                    must.Add(new QueryStringQuery() { Fields = new string[] { "ten" }, Query = term });
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
                SearchRequest req = new SearchRequest(_default_index)
                {
                    Query = new QueryContainer(new BoolQuery() {Must = must, MustNot = must_not}),
                    From = (page - 1) * page_size,
                    Size = page_size,
                    Sort = sort,
                    TrackTotalHits = true
                };
                var res = client.Search<ChungChi>(req);
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

        public ChungChi GetById(string id)
        {
            var obj = GetById<ChungChi>(_default_index, id);
            if (obj != null)
            {
                obj.id = id;
                return obj;
            }
            return null;
        }

        public bool UpdateThuocTinh(string id, List<int> thuoc_tinh)
        {
            return UpdateThuocTinh<ChungChi>(id, thuoc_tinh);
        }

        public List<ChungChi> GetAll(List<string> ids_ignore, string[] view_fields)
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
            var all = GetObjectScroll<ChungChi>(_default_index, query, source);
            return all.Select(HitToDocument).ToList();
        }
    }
}
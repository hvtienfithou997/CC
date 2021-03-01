using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiChungChiModels;

namespace ThiChungChiES
{
    public class DeThiTypingRepository : IESRepository
    {
        #region Init

        protected static string _default_index;

        //protected new ElasticClient client;
        private static DeThiTypingRepository instance;

        public DeThiTypingRepository(string modify_index)
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

        public static DeThiTypingRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    _default_index = "cc_de_thi_typing";
                    instance = new DeThiTypingRepository(_default_index);
                }
                return instance;
            }
        }

        public bool CreateIndex(bool delete_if_exist = false)
        {
            if (delete_if_exist && client.Indices.Exists(_default_index).Exists)
                client.Indices.Delete(_default_index);
            var createIndexResponse = client.Indices.Create(_default_index, s => s.Settings(st => st.NumberOfReplicas(0).NumberOfShards(3)).Map<CuocThi>(mm => mm.AutoMap().Dynamic(true)));
            return createIndexResponse.IsValid;
        }

        #endregion Init

        public bool Index(DeThiTyping data)
        {
            int retry = 0; int max_retry = 5;
            bool need_retry = true;
            if (!string.IsNullOrEmpty(data.word))
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

        public List<DeThiTyping> GetAll(List<string> ids_ignore, string[] view_fields)
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
            var all = GetObjectScroll<DeThiTyping>(_default_index, query, source);
            return all.Select(HitToDocument).ToList();
        }

        public DeThiTyping GetRandomDeThi()
        {
            var result = client.Search<DeThiTyping>(s => s
                .Query(q => q
                    .FunctionScore(fs => fs.Functions(f => f.RandomScore())
                        .Query(fq => fq.MatchAll()))));

            return result.Documents.First();
        }

        public DeThiTyping GetById(string id)
        {
            var obj = GetById<DeThiTyping>(_default_index, id);
            if (obj != null)
            {
                obj.id = id;
                return obj;
            }
            return null;
        }
        public bool Update(DeThiTyping data)
        {
            string id = $"{data.id}";
            data.id = string.Empty;
            data.ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            return Update(_default_index, data, id);
        }

        public bool Delete(string id)
        {
            return DeleteById<DeThiTyping>(id);
        }
        private DeThiTyping ConvertDoc(IHit<DeThiTyping> hit)
        {
            DeThiTyping u = new DeThiTyping();

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

        public List<DeThiTyping> Search(string term, string user, int page, out long total_recs, out string msg, int page_size = 50, bool is_admin = false)
        {
            msg = "";
            total_recs = 0;
            List<DeThiTyping> lst = new List<DeThiTyping>();
            try
            {
                List<QueryContainer> must = new List<QueryContainer>();
                List<QueryContainer> must_not = new List<QueryContainer>();
                if (!string.IsNullOrEmpty(term))
                {
                    must.Add(new QueryStringQuery() { Fields = new string[] { "ten"}, Query = term });
                }

                if (!is_admin)
                {
                    if (!string.IsNullOrEmpty(user))
                    {
                        must.Add(new TermQuery()
                        {
                            Field = "nguoi_tao.keyword",
                            Value = user
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
                var res = client.Search<DeThiTyping>(req);
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

        
    }
}
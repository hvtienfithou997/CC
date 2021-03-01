using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiChungChiModels;

namespace ThiChungChiES
{
    public class TaiKhoanRepository : IESRepository
    {
        #region Init

        protected static string _default_index;

        //protected new ElasticClient client;
        private static TaiKhoanRepository instance;

        public TaiKhoanRepository(string modify_index)
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

        public static TaiKhoanRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    _default_index = "cc_tai_khoan";
                    instance = new TaiKhoanRepository(_default_index);
                }
                return instance;
            }
        }

        public bool CreateIndex(bool delete_if_exist = false)
        {
            if (delete_if_exist && client.Indices.Exists(_default_index).Exists)
                client.Indices.Delete(_default_index);

            var createIndexResponse = client.Indices.Create(_default_index, s => s.Settings(st => st.NumberOfReplicas(0).NumberOfShards(3)).Map<TaiKhoan>(mm => mm.AutoMap().Dynamic(true)));
            return createIndexResponse.IsValid;
        }

        #endregion Init

        public bool Index(TaiKhoan data)
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

        public bool Update(TaiKhoan data)
        {
            string id = $"{data.id}";
            data.id = string.Empty;
            data.ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            return Update(_default_index, data, id);
        }

        public bool Delete(string id)
        {
            return DeleteById<TaiKhoan>(id);
        }

        private TaiKhoan ConvertDoc(IHit<TaiKhoan> hit)
        {
            TaiKhoan u = new TaiKhoan();

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

        public List<TaiKhoan> Search(string term,string user, int page, out long total_recs, out string msg, int page_size = 50, bool is_admin = false)
        {
            msg = "";
            total_recs = 0;
            List<TaiKhoan> lst = new List<TaiKhoan>();
            try
            {
                List<QueryContainer> must = new List<QueryContainer>();
                List<QueryContainer> must_not = new List<QueryContainer>();
                if (!string.IsNullOrEmpty(term))
                {
                    must.Add(new QueryStringQuery() { Fields = new string[] { "fullname", "username.keyword" }, Query = term });
                }

                if (!is_admin)
                {
                    if (!string.IsNullOrEmpty(user))
                    {
                        must.Add(new TermQuery()
                        {
                            Field = "username.keyword",
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
                var res = client.Search<TaiKhoan>(req);
                if (res.IsValid)
                {
                    total_recs = res.Total;
                    lst = res.Hits.Select(x => ConvertDoc(x)).ToList();
                }
            }
            catch (Exception ex)
            {
                msg = $"{ex.Message}. Trace: {ex.StackTrace}";
            }
            return lst;
        }

        public TaiKhoan GetById(string id)
        {
            var obj = GetById<TaiKhoan>(_default_index, id);
            if (obj != null)
            {
                obj.id = id;
                return obj;
            }
            return null;
        }

        public List<TaiKhoan> GetAll(List<string> ids_ignore, string[] view_fields)
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
            var all = GetObjectScroll<TaiKhoan>(_default_index, query, source);
            return all.Select(HitToDocument).ToList();
        }

        public TaiKhoan GetTaiKhoanByUsername(string username)
        {
            var re = client.Search<TaiKhoan>(s => s.Query(q => q.Term(t => t.Field("username.keyword").Value(username))).Size(1));
            if (re.Total > 0)
            {
                TaiKhoan user = re.Hits.First().Source;
                user.id = re.Hits.First().Id;
                return re.Documents.First();
            }
            return null;
        }

        public bool KiemTraTaiKhoanExist(string username)
        {
            var user = GetTaiKhoanByUsername(username);
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}
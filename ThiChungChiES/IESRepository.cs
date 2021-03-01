using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ThiChungChiModels;

namespace ThiChungChiES
{
    public class IESRepository
    {
        private static string reg_null_or_must_remove = "[\"][a-zA-Z0-9_]*[\"]:null[ ]*[,]?|[\"][a-zA-Z0-9_]*[\"]:" + -Int32.MaxValue + "[,]?";
        private static string reg_array_null = "\\[( *null *,? *)*]";
        protected static Uri node = new Uri(XMedia.XUtil.ConfigurationManager.AppSetting["ES:Server"]);
        protected ElasticClient client;
        protected static string formatDatePattern = "yyyy-MM-ddTHH:mm";
        protected static string formatDatePatternEndDay = "yyyy-MM-ddT23:59:59";
        protected static string formatDatePatternStartDay = "yyyy-MM-ddT00:00:00";
        protected static StickyConnectionPool connectionPool = new StickyConnectionPool(new[] { node });
        protected static double maxPriceValue = 3097976931348623;
        protected static uint cacheDate = 43200;
        protected static DateTime dateTime3000 = new DateTime(3000, 1, 1);
        protected static DateMath dateMathNow = DateMath.FromString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
        protected static DateMath dateMathEndDay = DateMath.FromString(DateTime.Now.ToString("yyyy-MM-ddT23:59:59"));
        protected static string NULL_VALUE = "_null_";

        public string EscapeTerm(string term)
        {
            string[] rmChars = new string[] { "-", "\"", "+", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "~", ":", "\\", "/" };
            if (term.Count(p => p == '"') % 2 == 0)
            {
                rmChars = new string[] { "-", "+", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "~", ":", "\\", "/" };
            }
            foreach (string item in rmChars)
            {
                term = term.Replace(item, " ");
            }

            return term;
        }

        public string RemoveCharNotAllow(string term)
        {
            string[] rmChars = new string[] { "-", "\"", "+", "=", "&&", "||", ">", "<", "!", "(", ")", "{", "}", "[", "]", "^", "~", ":", "\\", "/", ",", "?", "@", "#", "$", "%", "*", "." };

            foreach (string item in rmChars)
            {
                term = term.Replace(item, " ");
            }

            return term;
        }

        public bool ValidateQuery(string term)
        {
            var vali = new ValidateQueryRequest();
            vali.Query = new QueryContainer(new QueryStringQuery() { Query = term });
            return client.Indices.ValidateQuery(vali).Valid;
        }

        public int GetHashCode(string s)
        {
            int n = s.Length;
            int h = 0;
            for (int i = 0; i < n; i++)
            {
                h += s[i];
                h *= 123123123;
            }
            return h;
        }

        protected bool Index<T>(string _DefaultIndex, T data, string route, string id = "") where T : class
        {
            IndexRequest<object> req = new IndexRequest<object>(_DefaultIndex, typeof(T));
            if (!string.IsNullOrEmpty(route))
                req.Routing = route;
            req.Document = data;
            IndexResponse re = null;
            if (!string.IsNullOrEmpty(id))
                re = client.Index<T>(data, i => i.Id(id));
            else
                re = client.Index(req);
            return re.Result == Result.Created;
        }

        protected bool Index<T>(string _DefaultIndex, T data, string route, out string id) where T : class
        {
            id = "";
            IndexRequest<object> req = new IndexRequest<object>(_DefaultIndex, typeof(T));
            if (!string.IsNullOrEmpty(route))
                req.Routing = route;
            req.Document = data;
            IndexResponse re = null;

            re = client.Index(req);
            if (re.Result == Result.Created)
                id = re.Id;
            return re.Result == Result.Created;
        }

        protected bool Refresh(string _DefaultIndex)
        {
            var re = client.Indices.Refresh(_DefaultIndex, r => r.RequestConfiguration(c => c.RequestTimeout(TimeSpan.FromSeconds(5))));
            return re.IsValid;
        }

        public async Task<ConcurrentBag<IHit<T>>> GetObjectScrollAsync<T>(string _default_index, QueryContainer query, SourceFilter so, string scroll_timeout = "5m", int scroll_pageize = 2000) where T : class
        {
            if (query == null)
                query = new MatchAllQuery();
            if (so == null)
                so = new SourceFilter() { };
            ConcurrentBag<IHit<T>> bag = new ConcurrentBag<IHit<T>>();
            try
            {
                var searchResponse = await client.SearchAsync<T>(sd => sd.Source(s => so).Index(_default_index).From(0).Take(scroll_pageize).Query(q => query).Scroll(scroll_timeout));

                while (true)
                {
                    if (!searchResponse.IsValid || string.IsNullOrEmpty(searchResponse.ScrollId))
                        break;

                    if (!searchResponse.Documents.Any())
                        break;

                    var tmp = searchResponse.Hits;
                    foreach (var item in tmp)
                    {
                        bag.Add(item);
                    }
                    searchResponse = await client.ScrollAsync<T>(scroll_timeout, searchResponse.ScrollId);
                }

                await client.ClearScrollAsync(new ClearScrollRequest(searchResponse.ScrollId));
            }
            catch (Exception)
            {
            }
            finally
            {
            }
            return bag;
        }

        public ConcurrentBag<IHit<T>> GetObjectScroll<T>(string _default_index, QueryContainer query, SourceFilter so, string scroll_timeout = "5m", int scroll_pageize = 2000) where T : class
        {
            if (query == null)
                query = new MatchAllQuery();
            if (so == null)
                so = new SourceFilter() { };
            ConcurrentBag<IHit<T>> bag = new ConcurrentBag<IHit<T>>();
            try
            {
                var searchResponse = client.Search<T>(sd => sd.Source(s => so).Index(_default_index).From(0).Take(scroll_pageize).Query(q => query).Scroll(scroll_timeout));

                while (true)
                {
                    if (!searchResponse.IsValid || string.IsNullOrEmpty(searchResponse.ScrollId))
                        break;

                    if (!searchResponse.Documents.Any())
                        break;

                    var tmp = searchResponse.Hits;
                    foreach (var item in tmp)
                    {
                        bag.Add(item);
                    }
                    searchResponse = client.Scroll<T>(scroll_timeout, searchResponse.ScrollId);
                }

                client.ClearScroll(new ClearScrollRequest(searchResponse.ScrollId));
            }
            catch (Exception)
            {
            }
            finally
            {
            }
            return bag;
        }

        protected T HitToDocument<T>(IMultiGetHit<T> hit) where T : class, new()
        {
            if (hit.Found)
            {
                var doc = hit.Source;

                Type t = doc.GetType();
                PropertyInfo piShared = t.GetProperty("id");
                piShared.SetValue(doc, hit.Id);
                return doc;
            }
            return new T();
        }

        protected T HitToDocument<T>(IHit<T> hit) where T : class
        {
            var doc = hit.Source;

            Type t = doc.GetType();
            PropertyInfo piShared = t.GetProperty("id");
            if (piShared != null)
                piShared.SetValue(doc, hit.Id);
            return doc;
        }

        protected T GetById<T>(string _default_index, string id) where T : class
        {
            try
            {
                var g = new GetRequest(_default_index, id);
                var re = client.Get<T>(g);
                if (re.Found)
                    return re.Source;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public T GetById<T>(string id, string[] view_fields = null) where T : class
        {
            GetResponse<T> re;

            if (view_fields == null || view_fields.Contains("*"))
            {
                re = client.Get<T>(id);
            }
            else
            {
                re = client.Get<T>(id, g => g.SourceIncludes(view_fields));
            }

            if (re.Found)
            {
                var doc = re.Source;

                Type t = doc.GetType();
                PropertyInfo piShared = t.GetProperty("id");
                if (piShared != null)
                    piShared.SetValue(doc, re.Id);
                return doc;
            }

            return null;
        }

        public bool DeleteById<T>(string id) where T : class
        {
            var re = client.Delete<T>(id);
            return re.Result == Result.Deleted;
        }

        protected IEnumerable<T> GetAll<T>(string _default_index, string term, string[] search_fields, string[] view_fields, int page, int page_size, out long total_recs, List<QueryContainer> must_add = null) where T : class
        {
            total_recs = 0;

            List<QueryContainer> must = new List<QueryContainer>();
            if (must_add != null)
            {
                must.AddRange(must_add);
            }
            if (!string.IsNullOrEmpty(term))
            {
                must.Add(new QueryStringQuery()
                {
                    Fields = search_fields,
                    Query = term
                });
            }
            else
                must.Add(new MatchAllQuery());
            QueryContainer queryFilterMultikey = new QueryContainer(new BoolQuery()
            {
                Must = must
            });

            SourceFilter soF = new SourceFilter() { Includes = view_fields };
            if (view_fields.Contains("*"))
            {
                soF = new SourceFilter();
            }

            List<ISort> sort = new List<ISort>()
            {
            };
            if (!string.IsNullOrEmpty(term))
                sort.Add(new FieldSort() { Field = "_score", Order = SortOrder.Descending });
            SearchRequest request = new SearchRequest(Indices.Index(_default_index))
            {
                TrackTotalHits = true,
                Query = queryFilterMultikey,
                Source = soF,
                Sort = sort,
                Size = page_size,
                From = (page - 1) * page_size
            };
            var re = client.Search<T>(request);

            total_recs = re.Total;

            var lst = re.Hits.Select(x => HitToDocument(x));
            return lst;
        }

        protected bool Update<T>(string _default_index, T data, string id) where T : class
        {
            ConnectionSettings settings_update = new ConnectionSettings(connectionPool, sourceSerializer: Nest.JsonNetSerializer.JsonNetSerializer.Default).DefaultIndex(_default_index).DisableDirectStreaming(true);
            var client = new ElasticClient(settings_update);

            string json_doc = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            Regex regex = new Regex(reg_null_or_must_remove);
            json_doc = regex.Replace(json_doc, string.Empty);
            regex = new Regex(reg_array_null);
            json_doc = regex.Replace(json_doc, "[]");
            //json_doc = json_doc.Replace("\"ngay_tao\":0,", "").Replace("\"auto_id\":0,", "");
            var ob_up = Newtonsoft.Json.JsonConvert.DeserializeObject(json_doc);
            var re_up = client.Update<T, object>(id, u => u.Doc(ob_up));

            if (re_up.Result == Result.Error)
            {
                return Index<T>(_default_index, data, "", id);
            }
            return re_up.Result == Result.Updated;
        }

        protected bool Delete<T>(string _default_index, string id) where T : class
        {
            var re_up = client.Update<T, object>(id, u => u.Doc(new { trang_thai = TrangThai.DELETED }));
            return re_up.Result == Result.Updated || re_up.Result == Result.Noop;
        }

        public bool UpdateThuocTinh<T>(string id, List<int> thuoc_tinh) where T : class
        {
            var re = client.Update<T, object>(id, u => u.Doc(new { thuoc_tinh = thuoc_tinh }));
            return re.Result == Result.Updated || re.Result == Result.Noop;
        }
        protected bool IsOwner<T>(string _default_index, string id, string nguoi_tao) where T : class
        {
            try
            {
                var re = client.Get<T>(id, g => g.Index(_default_index).SourceIncludes(new string[] { "nguoi_tao" }));

                Type t = re.Source.GetType();

                PropertyInfo prop = t.GetProperty("nguoi_tao");

                if (prop != null)
                {
                    string owner = Convert.ToString(prop.GetValue(re.Source));
                    return owner == nguoi_tao;
                }
            }
            catch (Exception)
            {

            }

            return false;
        }
    }
}
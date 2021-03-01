using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThiChungChiModels;

namespace ThiChungChiES
{
    public class CuocThiRepository : IESRepository
    {
        #region Init

        protected static string _default_index;

        //protected new ElasticClient client;
        private static CuocThiRepository instance;

        public CuocThiRepository(string modify_index)
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

        public static CuocThiRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    _default_index = "cc_cuoc_thi";
                    instance = new CuocThiRepository(_default_index);
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

        public bool Index(CuocThi data)
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

        public bool Update(CuocThi data)
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
            return DeleteById<CuocThi>(id);
        }

        private CuocThi ConvertDoc(IHit<CuocThi> hit)
        {
            CuocThi u = new CuocThi();

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

        public List<CuocThi> Search(string term, IEnumerable<string> lst_should_id, long ngay_bd, long ngay_kt, string nguoi_tao, IEnumerable<int> thuoc_tinh, int page, out long total_recs, out string msg, int page_size = 50, bool is_admin = false)
        {
            msg = "";
            total_recs = 0;
            List<CuocThi> lst = new List<CuocThi>();
            try
            {
                List<QueryContainer> must = new List<QueryContainer>();
                List<QueryContainer> must_not = new List<QueryContainer>();
                List<QueryContainer> should = new List<QueryContainer>();
                if (!string.IsNullOrEmpty(term))
                {
                    must.Add(new QueryStringQuery() { Fields = new[] { "ten" }, Query = term });
                }

                if (thuoc_tinh.Count() > 0)
                {
                    must.Add(new TermsQuery()
                    {
                        Field = "thuoc_tinh",
                        Terms = thuoc_tinh.Select(x => (object)x)
                    });
                }

                if (lst_should_id.Count() > 0)
                {
                    should.Add(new IdsQuery
                    {
                        Values = lst_should_id.Where(x => !string.IsNullOrEmpty(x)).Select(x => (Id)x)
                    });
                }
                if (ngay_bd > 0)
                {
                    var dt_ngay_thi = XMedia.XUtil.EpochToTime(ngay_bd).Date;
                    must.Add(new LongRangeQuery()
                    {
                        Field = "ngay_bat_dau",
                        GreaterThanOrEqualTo = XMedia.XUtil.TimeInEpoch(dt_ngay_thi)
                    });
                    if (ngay_kt == 0)
                    {
                        var date = DateTimeOffset.MaxValue.Date;
                        must.Add(new LongRangeQuery()
                        {
                            Field = "ngay_ket_thuc",
                            LessThanOrEqualTo = XMedia.XUtil.TimeInEpoch(date)
                        });
                    }
                }

                if (ngay_kt > 0)
                {
                    var dt_ngay_thi = XMedia.XUtil.EpochToTime(ngay_kt).Date.AddSeconds(86400 - 1);
                    must.Add(new LongRangeQuery()
                    {
                        Field = "ngay_ket_thuc",
                        LessThanOrEqualTo = XMedia.XUtil.TimeInEpoch(dt_ngay_thi)
                    });
                    if (ngay_bd == 0)
                    {
                        var date = DateTimeOffset.MinValue.Date;
                        must.Add(new LongRangeQuery()
                        {
                            Field = "ngay_ket_thuc",
                            GreaterThanOrEqualTo = XMedia.XUtil.TimeInEpoch(date)
                        });
                    }
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
                req.Query = new QueryContainer(new BoolQuery() { Must = must, MustNot = must_not, Should = should });
                req.From = (page - 1) * page_size;
                req.Size = page_size;
                req.Sort = sort;
                req.TrackTotalHits = true;
                var res = client.Search<CuocThi>(req);
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

        public List<CuocThi> GetAll(List<string> ids_ignore, string[] view_fields, string nguoi_tao, bool is_admin = false)
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
                if (!string.IsNullOrEmpty(nguoi_tao))
                {
                    must.Add(new TermQuery()
                    {
                        Field = "nguoi_tao.keyword",
                        Value = nguoi_tao
                    });
                }
            }
            var query = new QueryContainer(new BoolQuery() { Must = must, MustNot = must_not });
            var source = new SourceFilter() { Includes = view_fields };
            var all = GetObjectScroll<CuocThi>(_default_index, query, source);
            return all.Select(HitToDocument).ToList();
        }

        public List<CuocThi> GetAll(List<string> ids_ignore, string[] view_fields, bool is_admin = false)
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
            var all = GetObjectScroll<CuocThi>(_default_index, query, source);
            return all.Select(HitToDocument).ToList();
        }

        public CuocThi GetById(string id)
        {
            var obj = GetById<CuocThi>(_default_index, id);
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

        public List<CuocThi> SearchByPeriod(long ngay_bd, long ngay_kt, bool is_admin = false)
        {
            List<QueryContainer> must = new List<QueryContainer>();

            if (ngay_bd > 0)
            {
                var dt_ngay_thi = XMedia.XUtil.EpochToTime(ngay_bd).Date;
                must.Add(new LongRangeQuery()
                {
                    Field = "ngay_bat_dau",
                    GreaterThanOrEqualTo = XMedia.XUtil.TimeInEpoch(dt_ngay_thi)
                });
                if (ngay_kt == 0)
                {
                    var date = DateTimeOffset.MaxValue.Date;
                    must.Add(new LongRangeQuery()
                    {
                        Field = "ngay_ket_thuc",
                        LessThanOrEqualTo = XMedia.XUtil.TimeInEpoch(date)
                    });
                }
            }

            if (ngay_kt > 0)
            {
                var dt_ngay_thi = XMedia.XUtil.EpochToTime(ngay_kt).Date.AddSeconds(86400 - 1);
                must.Add(new LongRangeQuery()
                {
                    Field = "ngay_ket_thuc",
                    LessThanOrEqualTo = XMedia.XUtil.TimeInEpoch(dt_ngay_thi)
                });
                if (ngay_bd == 0)
                {
                    var date = DateTimeOffset.MinValue.Date;
                    must.Add(new LongRangeQuery()
                    {
                        Field = "ngay_ket_thuc",
                        GreaterThanOrEqualTo = XMedia.XUtil.TimeInEpoch(date)
                    });
                }
            }
            var query = new QueryContainer(new BoolQuery() { Must = must });
            var source = new SourceFilter() { Includes = null };
            var all = GetObjectScroll<CuocThi>(_default_index, query, source);
            return all.Select(HitToDocument).ToList();
        }

        public bool dele()
        {
            var re = client.DeleteByQuery<DanhSachThi>(q => q.Index(_default_index).MatchAll());
            return false;
        }
    }
}
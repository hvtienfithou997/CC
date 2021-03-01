using Nest;
using ThiChungChiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ThiChungChiES
{
    public class TaiKhoanChungChiRepository: IESRepository
    {
        #region Init
        protected static string _default_index;
        //protected new ElasticClient client;
        private static TaiKhoanChungChiRepository instance;
        public TaiKhoanChungChiRepository(string modify_index)
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
        public static TaiKhoanChungChiRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    _default_index = "cc_tai_khoan_chung_chi";
                    instance = new TaiKhoanChungChiRepository(_default_index);
                }
                return instance;
            }
        }
        public bool CreateIndex(bool delete_if_exist = false)
        {
            if (delete_if_exist && client.Indices.Exists(_default_index).Exists)
                client.Indices.Delete(_default_index);

            var createIndexResponse = client.Indices.Create(_default_index, s => s.Settings(st => st.NumberOfReplicas(0).NumberOfShards(3)).Map<TaiKhoanChungChi>(mm => mm.AutoMap().Dynamic(true)));
            return createIndexResponse.IsValid;
        }
        #endregion
        public bool Index(TaiKhoanChungChi data)
        {
            int retry = 0; int max_retry = 5;
            bool need_retry = true;
            while (retry++ < max_retry && need_retry)
            {
                need_retry = !Index(_default_index, data, "", data.id);
                if (need_retry)
                    Task.Delay(1000).Wait();
            }
            return !need_retry;
        }
        public bool Update(TaiKhoanChungChi data)
        {
            string id = $"{data.id}";
            data.id = string.Empty;
            data.ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);

            return Update(_default_index, data, id);
        }
        private TaiKhoanChungChi ConvertDoc(IHit<TaiKhoanChungChi> hit)
        {
            TaiKhoanChungChi u = new TaiKhoanChungChi();

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
        public List<TaiKhoanChungChi> Search(string term, int page, out long total_recs, out string msg, int page_size = 50)
        {
            msg = "";
            total_recs = 0;
            List<TaiKhoanChungChi> lst = new List<TaiKhoanChungChi>();
            try
            {
                List<QueryContainer> must = new List<QueryContainer>();
                List<QueryContainer> must_not = new List<QueryContainer>();
                if (!string.IsNullOrEmpty(term))
                {
                    must.Add(new QueryStringQuery() { Fields = new string[] { "fullname", "username" }, Query = term });
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
                var res = client.Search<TaiKhoanChungChi>(req);
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
        public TaiKhoanChungChi GetById(string id)
        {
            var obj = GetById<TaiKhoanChungChi>(_default_index, id);
            if (obj != null)
            {
                obj.id = id;
                return obj;
            }
            return null;
        }
        public bool Delete(string id)
        {
            return DeleteById<TaiKhoanChungChi>(id);
        }



    }
}

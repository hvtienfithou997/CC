using Nest;
using ThiChungChiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThiChungChiES
{
    public class KieuKetQuaRepository: IESRepository
    {
        #region Init
        protected static string _default_index;
        //protected new ElasticClient client;
        private static KieuKetQuaRepository instance;
        public KieuKetQuaRepository(string modify_index)
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
        public static KieuKetQuaRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    _default_index = "cc_kieu_ket_qua";
                    instance = new KieuKetQuaRepository(_default_index);
                }
                return instance;
            }
        }
        public bool CreateIndex(bool delete_if_exist = false)
        {
            if (delete_if_exist && client.Indices.Exists(_default_index).Exists)
                client.Indices.Delete(_default_index);

            var createIndexResponse = client.Indices.Create(_default_index, s => s.Settings(st => st.NumberOfReplicas(0).NumberOfShards(3)).Map<KieuKetQua>(mm => mm.AutoMap().Dynamic(true)));
            return createIndexResponse.IsValid;
        }
        #endregion
        public bool Index(KieuKetQua data)
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
        public bool Update(KieuKetQua data)
        {
            string id = "";
            //data.id = string.Empty;
            //data.ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);

            return Update(_default_index, data, id);
        }
        public bool Delete(string id)
        {
            return DeleteById<KieuKetQua>(id);
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThiChungChiModels
{
    public class TaiKhoanChungChi : BaseModel
    {
        [Display(Name = "Tài khoản")]
        public string id_tai_khoan { get; set; }

        [Display(Name = "Chứng chỉ")]
        public string id_chung_chi { get; set; }

        [Display(Name = "Kết quả thi")]
        public string id_ket_qua_thi { get; set; }

        [Display(Name = "Ngày cấp")]
        public int ngay_cap { get; set; }

        /// <summary>
        /// Nội dung in trên chứng chỉ
        /// </summary>
        [Display(Name = "Nội dung")]
        public string noi_dung { get; set; }

        [Display(Name = "Thuộc tính")]
        public List<int> thuoc_tinh { get; set; }

        [Display(Name = "Địa chỉ nhận Offline")]
        public string dia_chi_nhan_offline { get; set; }

        [Display(Name = "Ngày gửi Offline")]
        public int ngay_gui_offline { get; set; }
    }
}
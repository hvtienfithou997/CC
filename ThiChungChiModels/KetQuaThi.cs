using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThiChungChiModels
{
    public class KetQuaThi : BaseModel
    {
        [Display(Name = "Cuộc thi")]
        public string id_cuoc_thi { get; set; }

        [Display(Name = "Tài khoản")]
        public string id_tai_khoan { get; set; }

        [Display(Name = "Ngày thi")]
        public int ngay_thi { get; set; }

        /// <summary>
        /// Điểm dạng double dùng để sort tính toán phân hạng cao thấp
        /// </summary>
        [Display(Name = "Điểm thi")]
        public double diem_thi { get; set; }

        /// <summary>
        /// Kết quả thi dạng JSON: {'diem':'100/120','thoi_gian_lam_bai':'60/60','ngay_lam_bai':EPOCH_TIME} -> hiển thị khi xem chi tiết kết quả
        /// </summary>
        [Display(Name = "Kết quả thi")]
        public string ket_qua_thi_json { get; set; }

        [Display(Name = "Thuộc tính")]
        public List<int> thuoc_tinh { get; set; }

        public KetQuaThi()
        {
            ngay_thi = Convert.ToInt32(XMedia.XUtil.TimeInEpoch(DateTime.Now));
            ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
        }
    }
}
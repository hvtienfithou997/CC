using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThiChungChiModels
{
    public class ChungChi : BaseModel
    {
        [Display(Name = "Tên chứng chỉ")]
        public string ten { get; set; }

        [Display(Name = "Thuộc tính")]
        public List<int> thuoc_tinh { get; set; }

        /// <summary>
        /// Mẫu chứng chỉ (TOP 10 toàn quốc, Nhân viên xuất sắc, Vô địch quốc gia....) -> dùng để load các mẫu chứng chỉ hiển thị hoặc in ấn
        /// </summary>
        [Display(Name = "Mẫu chứng chỉ")]
        public int mau_chung_chi { get; set; }

        [Display(Name = "Nội dung")]
        public string noi_dung { get; set; }

        public ChungChi()
        {
            ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
        }
    }
}
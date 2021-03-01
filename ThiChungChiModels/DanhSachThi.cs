using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThiChungChiModels
{
    public class DanhSachThi:BaseModel
    {
        [Display(Name = "Cuộc thi")]
        public string id_cuoc_thi { get; set; }
        [Display(Name = "Tài khoản")]
        public string id_tai_khoan { get; set; }
        [Display(Name = "Thuộc tính")]
        public List<int> thuoc_tinh { get; set; }
        public DanhSachThi()
        {
            ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
        }
    }
}

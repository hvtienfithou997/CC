using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThiChungChiModels
{
    public class TaiKhoan : BaseModel
    {
        [Display(Name = "Tài khoản")]
        public string username { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Tên đầy đủ")]
        public string fullname { get; set; }

        [Display(Name = "Thuộc tính")]
        public List<int> thuoc_tinh { get; set; }

        [Display(Name = "Đăng nhập cuối")]
        public int dang_nhap_cuoi { get; set; }

        public int role { get; set; }

        public TaiKhoan()
        {
            ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
        }
    }
}
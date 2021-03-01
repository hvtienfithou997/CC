using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThiChungChiModels
{
    public class KieuKetQua
    {
        [Display(Name = "Tên")]
        public string ten { get; set; }
        [Display(Name = "MIN")]
        public int min { get; set; }
        [Display(Name = "MAX")]
        public int max { get; set; }
        [Display(Name = "Kiểu dữ liệu")]
        public KieuDuLieuKetQua kieu_du_lieu { get; set; }
    }
    public enum KieuDuLieuKetQua
    {
        [Display(Name = "Chẵn")]
        INT,
        [Display(Name = "Lẻ")]
        DOUBLE,
        [Display(Name = "Đang nghĩ")]
        BOOL
    }
}

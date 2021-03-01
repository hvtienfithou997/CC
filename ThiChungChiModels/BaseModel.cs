using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThiChungChiModels
{
    public class BaseModel
    {
        [Keyword]
        public string id { get; set; }
        [Display(Name = "Ngày tạo")]
        public long ngay_tao { get; set; }
        [Display(Name = "Ngày sửa")]
        public long ngay_sua { get; set; }
        [Display(Name = "Người tạo")]
        [Keyword]
        public string nguoi_tao { get; set; }
        [Display(Name = "Người sửa")]
        [Keyword]
        public string nguoi_sua { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThiChungChiModels
{
    public class DeThiTyping:BaseModel
    {
        public DeThiTyping()
        {
            ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
        }
        [Display(Name = "Đề thi")]
        public string word { get; set; }
        [Display(Name = "Tên đề")]
        public string ten { get; set; }
        [Display(Name = "Thời gian thi (s)")]
        public int time { get; set; }
    }
}

using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThiChungChiModels
{
    public class CuocThi : BaseModel
    {
        [Display(Name = "Danh mục cha")]
        [Keyword]
        public string id_cha { get; set; }
        [Display(Name = "Tên cuộc thi")]

        public string ten { get; set; }
        [Display(Name = "Chứng chỉ")]
        public string id_chung_chi { get; set; }

        [Display(Name = "Mật khẩu cuộc thi")]
        [Text(Index = false)]
        public string pass_thi { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public int ngay_bat_dau { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public int ngay_ket_thuc { get; set; }

        /// <summary>
        /// Các thuộc tính như: Tỉnh, Quận huyện, Miền Bắc Nam, Giáo Viên, Học sinh, Trạng thái cuộc thi (mở, đóng)....
        /// </summary>
        [Display(Name = "Thuộc tính")]
        public List<int> thuoc_tinh { get; set; }
        public long so_lan_thi_lai { get; set; }
        /// <summary>
        /// Định nghĩa kiểu kết quả sẽ nhận từ MODULE khác: [{'ten':'Hạng A','min': 80, 'max': 100, 'kieu_du_lieu':'int'},{'ten':'Hạng B','min': 50, 'max': 80, 'kieu_du_lieu':'int'},,{'ten':'Hạng C','min': 0, 'max': 50, 'kieu_du_lieu':'int'}]
        /// </summary>
        [Display(Name = "Kiểu kết quả")]
        public List<KieuKetQua> kieu_ket_qua { get; set; }

        [Display(Name = "Loại cuộc thi")]
        public LoaiCuocThi loai_cuoc_thi { get; set; }
        public string id_de_thi { get; set; }

        public string noi_dung { get; set; }

        public CuocThi()
        {
            ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
        }
    }
   
}
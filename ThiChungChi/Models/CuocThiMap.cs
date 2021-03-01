using Nest;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ThiChungChiModels;

namespace ThiChungChi.Models
{
    public class CuocThiMap : BaseModel
    {
        public CuocThiMap(CuocThi ct, List<ThuocTinh> tt, Dictionary<string, string> dm_cuoc_thi, string ten_de_thi, bool expired)
        {
            if (!string.IsNullOrEmpty(ct.id_cha))
            {
                foreach (var dm in dm_cuoc_thi.Where(x => x.Key == ct.id_cha))
                {
                    ten_dm_cuoc_thi = dm.Value;
                }
            }

            this.expired = expired;
            id = ct.id;
            id_cha = ct.id_cha;
            ten_dm_cha = ten_dm_cuoc_thi;
            ten = ct.ten;
            pass_thi = ct.pass_thi;
            ngay_bat_dau = ct.ngay_bat_dau;
            ngay_ket_thuc = ct.ngay_ket_thuc;
            ngay_tao = ct.ngay_tao;
            ngay_sua = ct.ngay_sua;
            nguoi_tao = ct.nguoi_tao;
            nguoi_sua = ct.nguoi_sua;
            var item_thuoc_tinh = new List<int>();
            if (ct.thuoc_tinh != null)
                item_thuoc_tinh = ct.thuoc_tinh;
            thuoc_tinh = tt.Where(x => item_thuoc_tinh.Contains(x.gia_tri)).ToDictionary(x => x.gia_tri, y => y.ten);
            loai_cuoc_thi = ct.loai_cuoc_thi;
            id_de_thi = ct.id_de_thi;
            this.ten_de_thi = ten_de_thi;
            noi_dung = ct.noi_dung;
        }
        public string noi_dung { get; set; }

        public bool expired { get; set; }

        [Display(Name = "Tên đề thi")]
        public string ten_de_thi { get; set; }

        [Display(Name = "Danh mục cha")]
        [Keyword]
        public string id_cha { get; set; }

        [Display(Name = "Danh mục cha")]
        public string ten_dm_cuoc_thi { get; set; }

        public string ten_dm_cha { get; set; }

        [Display(Name = "Tên cuộc thi")]
        public string ten { get; set; }

        [Display(Name = "Mật khẩu cuộc thi")]
        [Text(Index = false)]
        public string pass_thi { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public int ngay_bat_dau { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public long ngay_ket_thuc { get; set; }

        [Display(Name = "Loại cuộc thi")]
        public LoaiCuocThi loai_cuoc_thi { get; set; }

        [Display(Name = "Đề thi")]
        public string id_de_thi { get; set; }

        /// <summary>
        /// Các thuộc tính như: Tỉnh, Quận huyện, Miền Bắc Nam, Giáo Viên, Học sinh, Trạng thái cuộc thi (mở, đóng)....
        /// </summary>
        [Display(Name = "Thuộc tính")]
        public Dictionary<int, string> thuoc_tinh { get; set; }

        /// <summary>
        /// Định nghĩa kiểu kết quả sẽ nhận từ MODULE khác: {'ten':'Hạng A','min': 80, 'max': 100, 'kieu_du_lieu':'int'},{'ten':'Hạng B','min': 50, 'max': 80, 'kieu_du_lieu':'int'}
        /// </summary>
        [Display(Name = "Kiểu kết quả")]
        public KieuKetQua kieu_ket_qua { get; set; }
    }
}
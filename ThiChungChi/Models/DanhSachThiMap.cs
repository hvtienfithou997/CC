using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ThiChungChiModels;

namespace ThiChungChi.Models
{
    public class DanhSachThiMap : BaseModel
    {
        public DanhSachThiMap(CuocThi ct, TaiKhoan tk, DanhSachThi ds)
        {
            id = ds.id;
            id_cuoc_thi = ct.id;
            ten_cuoc_thi = ct.ten;
            id_tai_khoan = tk.id;
            ten_tai_khoan = tk.username;
            ngay_tao = ds.ngay_tao;
            ngay_sua = ds.ngay_sua;
            nguoi_sua = ds.nguoi_sua;
            nguoi_tao = ds.nguoi_tao;
            ngay_bat_dau = ct.ngay_bat_dau;
            ngay_ket_thuc = ct.ngay_ket_thuc;
        }

        public DanhSachThiMap(CuocThi ct, TaiKhoan tk, DanhSachThi ds, List<ThuocTinh> tt, bool exam,bool expired, bool start, bool luot_thi)
        {
            id = ds.id;
            if (ct != null)
            {
                id_cuoc_thi = ct.id;
                ten_cuoc_thi = ct.ten;
                ngay_bat_dau = ct.ngay_bat_dau;
                ngay_ket_thuc = ct.ngay_ket_thuc;
            }
            if (tk != null)
            {
                id_tai_khoan = tk.username;
                ten_tai_khoan = tk.username;
                fullname = tk.fullname;
            }
            var item_thuoc_tinh = new List<int>();
            if (ds.thuoc_tinh != null)
                item_thuoc_tinh = ds.thuoc_tinh;
            thuoc_tinh = tt.Where(x => item_thuoc_tinh.Contains(x.gia_tri)).ToDictionary(x => x.gia_tri, y => y.ten);
            ngay_tao = ds.ngay_tao;
            ngay_sua = ds.ngay_sua;
            nguoi_sua = ds.nguoi_sua;
            nguoi_tao = ds.nguoi_tao;
            tinh_trang_thi = exam;
            is_expired = expired;
            this.start = start;
            so_lan_thi_lai = ct.so_lan_thi_lai;
            this.luot_thi = luot_thi;
            this.pass_thi = ct.pass_thi;
        }

        public string pass_thi { get; set; }

        public bool luot_thi { get; set; }
        public long so_lan_thi_lai { get; set; }
        public bool start { get; set; }
        public bool is_expired { get; set; }

        [Display(Name = "Tình trạng")]
        public bool tinh_trang_thi { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public int ngay_bat_dau { get; set; }
        [Display(Name = "Ngày kết thúc")]
        public int ngay_ket_thuc { get; set; }

        public string id_cuoc_thi { get; set; }

        [Display(Name = "Tên cuộc thi")]
        public string ten_cuoc_thi { get; set; }

        public string id_tai_khoan { get; set; }

        [Display(Name = "Tên tài khoản")]
        public string ten_tai_khoan { get; set; }

        [Display(Name = "Tên đầy đủ")]
        public string fullname { get; set; }

        [Display(Name = "Thuộc tính")]
        public Dictionary<int, string> thuoc_tinh { get; set; }
    }

    public class DanhSach : BaseModel
    {
        public DanhSach(CuocThi ct, TaiKhoan tk, DanhSachThi ds)
        {
            id = ds.id;
            id_cuoc_thi = ct.id;
            ten_cuoc_thi = ct.ten;
            if (tk != null)
            {
                id_tai_khoan = tk.id;
                ten_tai_khoan = tk.username;
                fullname = tk.fullname;
            }
            ngay_tao = ds.ngay_tao;
            ngay_sua = ds.ngay_sua;
            nguoi_sua = ds.nguoi_sua;
            nguoi_tao = ds.nguoi_tao;
        }

        public string id_cuoc_thi { get; set; }

        [Display(Name = "Tên cuộc thi")]
        public string ten_cuoc_thi { get; set; }

        public string id_tai_khoan { get; set; }

        [Display(Name = "Tên tài khoản")]
        public string ten_tai_khoan { get; set; }

        [Display(Name = "Thuộc tính")]
        public string thuoc_tinh { get; set; }

        [Display(Name = "Tên đầy đủ")]
        public string fullname { get; set; }
    }
}
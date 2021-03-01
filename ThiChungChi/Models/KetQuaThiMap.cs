using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ThiChungChiModels;

namespace ThiChungChi.Models
{
    public class KetQuaThiMap : BaseModel
    {
        public KetQuaThiMap(KetQuaThi kq, TaiKhoan tk, CuocThi ct, List<ThuocTinh> tt)
        {
            id = kq.id;
            nguoi_tao = kq.nguoi_tao;
            ngay_tao = kq.ngay_tao;
            nguoi_sua = kq.nguoi_sua;
            ngay_sua = kq.ngay_sua;
            ngay_thi = kq.ngay_thi;
            if (tk != null)
            {
                id_tai_khoan = tk.id;
                username = tk.username;
            }

            if (ct != null)
            {
                id_cuoc_thi = ct.id;
                ten_cuoc_thi = ct.ten;
            }
            
            diem_thi = kq.diem_thi;
            ket_qua_thi_json = kq.ket_qua_thi_json;
            var item_thuoc_tinh = new List<int>();
            if (kq.thuoc_tinh != null)
                item_thuoc_tinh = kq.thuoc_tinh;
            thuoc_tinh = tt.Where(x => item_thuoc_tinh.Contains(x.gia_tri)).ToDictionary(x => x.gia_tri, y => y.ten);
        }

        [Display(Name = "Cuộc thi")]
        public string id_cuoc_thi { get; set; }

        [Display(Name = "Tên cuộc thi")]
        public string ten_cuoc_thi { get; set; }

        [Display(Name = "Tài khoản")]
        public string id_tai_khoan { get; set; }

        [Display(Name = "User name")]
        public string username { get; set; }

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
        public Dictionary<int, string> thuoc_tinh { get; set; }
    }
}
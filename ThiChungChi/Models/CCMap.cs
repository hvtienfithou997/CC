using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ThiChungChiModels;

namespace ThiChungChi.Models
{
    public class CCMap : BaseModel
    {
        public CCMap(ChungChi item, List<ThuocTinh> tt)
        {
            ten = item.ten;
            mau_chung_chi = item.mau_chung_chi;
            ngay_sua = item.ngay_sua;
            ngay_tao = item.ngay_tao;
            nguoi_tao = item.nguoi_tao;
            nguoi_sua = item.nguoi_sua;
            id = item.id;
            var item_thuoc_tinh = new List<int>();
            if (item.thuoc_tinh != null)
                item_thuoc_tinh = item.thuoc_tinh;
            thuoc_tinh = tt.Where(x => item_thuoc_tinh.Contains(x.gia_tri)).ToDictionary(x => x.gia_tri, y => y.ten);
            noi_dung = item.noi_dung;

        }
        [Display(Name = "Nội dung")]
        public string noi_dung { get; set; }

        [Display(Name = "Tên chứng chỉ")]
        public string ten { get; set; }

        [Display(Name = "Thuộc tính")]
        public Dictionary<int, string> thuoc_tinh { get; set; }

        [Display(Name = "Mẫu chứng chỉ")]
        public int mau_chung_chi { get; set; }
    }
}
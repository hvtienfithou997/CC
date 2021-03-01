using System;
using System.ComponentModel.DataAnnotations;

namespace ThiChungChiModels
{
    public class ThuocTinh : BaseModel
    {
        public ThuocTinh()
        {
            ngay_tao = XMedia.XUtil.TimeInEpoch(DateTime.Now);
            ngay_sua = XMedia.XUtil.TimeInEpoch(DateTime.Now);
        }

        [Display(Name = "Loại")]
        public LoaiThuocTinh loai { get; set; }

        [Display(Name = "Giá trị")]
        public int gia_tri { get; set; }

        [Display(Name = "Tên")]
        public string ten { get; set; }

        [Display(Name = "Nhóm")]
        public int nhom { get; set; }

        [Display(Name = "Loại thuộc tính")]
        public ThuocTinhType type { get; set; }

        [Display(Name = "Thuộc tính của hệ thống")]
        public bool is_system { get; set; }
    }

    public enum LoaiThuocTinh
    {
        [Display(Name = "Tất cả")]
        ALL,

        [Display(Name = "Tài khoản")]
        TAI_KHOAN,

        [Display(Name = "Cuộc Thi")]
        CUOC_THI,

        [Display(Name = "Danh sách thi")]
        DANH_SACH_THI,

        [Display(Name = "Kết quả thi")]
        KET_QUA_THI,

        [Display(Name = "Chứng chỉ")]
        CHUNG_CHI,

        [Display(Name = "Tài khoản - Chứng chỉ")]
        TAI_KHOAN_CHUNG_CHI
    }

    public enum ThuocTinhType
    {
        [Display(Name = "Shared")]
        SHARED,

        [Display(Name = "Private")]
        PRIVATE
    }
}
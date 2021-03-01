
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ThiChungChiModels
{

    public enum TrangThai
    {
        ACTIVE, DISABLED, DELETED
    }

    public enum LoaiCuocThi
    {
        [Display(Name = "Đánh máy")]
        DANHMAY,
        [Display(Name = "Trắc nghiệm")]
        TRACNGHIEM
    }


    public enum Role
    {
        USER,
        ADMIN,
        CREATE
    }

}

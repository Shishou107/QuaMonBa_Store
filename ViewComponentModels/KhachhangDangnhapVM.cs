using System.ComponentModel.DataAnnotations;

namespace QuaMonBa_Store.ViewComponentModels
{
    public class KhachhangDangnhapVM
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; } = string.Empty;
    }
}

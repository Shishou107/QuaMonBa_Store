using System;
using System.ComponentModel.DataAnnotations;

namespace QuaMonBa_Store.ViewComponentModels
{
    public class KhachhangVM
    {
        [Display(Name = "Mã khách hàng")]
        [Required(ErrorMessage = "Mã khách hàng không được để trống")]
        public string MaKh { get; set; } = string.Empty;

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)] // Giúp thẻ <input> tự động chuyển thành type="password" (ẩn ký tự)
        public string MatKhau { get; set; } = string.Empty;

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MaxLength(50, ErrorMessage = "Họ tên không được vượt quá 50 ký tự")]
        public string HoTen { get; set; } = string.Empty;

        [Display(Name = "Giới tính")]
        public bool GioiTinh { get; set; } = true; // Mặc định true (Nữ) như bạn thiết lập

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)] // Giúp thẻ <input> tự động hiển thị bộ chọn lịch (Datepicker)
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [MaxLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string DiaChi { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [MaxLength(10, ErrorMessage = "Số điện thoại không được vượt quá 10 ký tự")]
        // Đã sửa lại Regex chuẩn cho số điện thoại VN: Bắt đầu bằng 0, theo sau là 9-10 chữ số
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ (phải bắt đầu bằng số 0 và có 10-11 số)")]
        public string DienThoai { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using QuaMonBa_Store.Data;
using QuaMonBa_Store.Helpers;
using QuaMonBa_Store.ViewComponentModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuaMonBa_Store.Controllers
{
    public class KhachhangController : Controller
    {
        private readonly Hshop2023Context _context;
        private readonly IMapper _mapper;
        public KhachhangController(Hshop2023Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dangky()
        {
            return View("Index");
        }
        [HttpPost]
        public IActionResult Dangky(KhachhangVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var khachHang = _mapper.Map<KhachHang>(model);
                    khachHang.RandomKey = CreateSalt.GenerateSalt();
                    khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                    khachHang.HieuLuc = true;
                    khachHang.VaiTro = 0;

                    if (_context.KhachHangs.Any(kh => kh.MaKh == khachHang.MaKh))
                    {
                        ModelState.AddModelError("MaKh", "Mã khách hàng đã tồn tại");

                        // NẾU LỖI: Đánh dấu form Đăng ký và giữ lại dữ liệu
                        ViewBag.ActiveForm = "Register";
                        ViewBag.RegisterModel = model;
                        return View("Index"); // Trả về trang chính
                    }

                    _context.Add(khachHang);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    var mess = $"{ex.Message} shh";
                }
            }

            // NẾU LỖI VALIDATION: Đánh dấu form Đăng ký và giữ lại dữ liệu
            ViewBag.ActiveForm = "Register";
            ViewBag.RegisterModel = model;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Dangnhap(string? returnUrl, KhachhangDangnhapVM model)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var khachHang = _context.KhachHangs.FirstOrDefault(kh => kh.MaKh == model.TenDangNhap);

                if (khachHang == null)
                    ModelState.AddModelError(string.Empty, "Mã khách hàng không tồn tại");
                else if (!khachHang.HieuLuc)
                    ModelState.AddModelError(string.Empty, "Tài khoản không còn hiệu lực");
                else if (model.MatKhau.ToMd5Hash(khachHang.RandomKey) != khachHang.MatKhau)
                    ModelState.AddModelError(string.Empty, "Mật khẩu không đúng");
                else
                {
                    // ... (Code đăng nhập thành công của bạn giữ nguyên) ...
                    string quyenTruyCap = (khachHang.VaiTro == 0) ? "User" : "Admin";
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, khachHang.Email),
                new Claim(ClaimTypes.Name, khachHang.HoTen),
                new Claim("VaiTro", khachHang.MaKh),
                new Claim(ClaimTypes.Role, quyenTruyCap)
            };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    else return Redirect("/");
                }
            }

            // NẾU LỖI ĐĂNG NHẬP: Đánh dấu form Đăng nhập và giữ lại dữ liệu
            ViewBag.ActiveForm = "Login";
            ViewBag.LoginModel = model;
            return View("Index");
        }
        [Authorize]

        public async Task<IActionResult> DangXuat()

        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");

        }
    }
}

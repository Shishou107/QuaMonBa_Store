using Microsoft.AspNetCore.Mvc;
using QuaMonBa_Store.data;
using QuaMonBa_Store.Data;
using QuaMonBa_Store.Helpers;
using System.Text.Json;

namespace QuaMonBa_Store.Controllers
{
    public class ShopcartController : Controller
    {
        private readonly Hshop2023Context _context;
        public ShopcartController(Hshop2023Context context) => _context = context;

        // 1. CHÌA KHÓA: Hàm gọi Giỏ hàng từ Session ra (đỡ phải viết đi viết lại)
        public List<GioHang> Cart
        {
            get
            {
                var data = HttpContext.Session.Get<List<GioHang>>("GioHang");
                if (data == null)
                {
                    data = new List<GioHang>();
                }
                return data;
            }
        }

        // 2. TRANG CHỦ GIỎ HÀNG: Hiển thị các món đang có
        public IActionResult Index()
        {
            return View(Cart);
        }

        // 3. THÊM VÀO GIỎ HÀNG
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var myCart = Cart;
            var item = myCart.SingleOrDefault(p => p.MaHH == id);

            if (item == null) // Nếu món này chưa có trong giỏ
            {
                var hangHoa = _context.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    return NotFound("Không tìm thấy sản phẩm");
                }

                item = new GioHang
                {
                    MaHH = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? "default.jpg",
                    SoLuong = quantity
                };
                myCart.Add(item);
            }
            else // Nếu món này đã có trong giỏ thì chỉ tăng số lượng
            {
                item.SoLuong += quantity;
            }

            // Lưu lại giỏ hàng vào Session
            HttpContext.Session.Set("GioHang", myCart);

            return RedirectToAction("Index"); // Thêm xong chuyển thẳng sang trang Giỏ hàng
        }

        // 4. XÓA MÓN HÀNG KHỎI GIỎ
        public IActionResult RemoveCartItem(int id)
        {
            var myCart = Cart;
            var item = myCart.SingleOrDefault(p => p.MaHH == id);

            if (item != null)
            {
                myCart.Remove(item);
                HttpContext.Session.Set("GioHang", myCart); // Nhớ cập nhật lại Session sau khi xóa
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdateQuantity(int id, string type)
        {
            // Cực kỳ ngắn gọn, khử luôn warning bằng ?? new List<GioHang>()
            var cart = HttpContext.Session.Get<List<GioHang>>("GioHang") ?? new List<GioHang>();

            var item = cart.FirstOrDefault(p => p.MaHH == id);

            if (item != null)
            {
                if (type == "plus")
                {
                    item.SoLuong++;
                }
                else if (type == "minus")
                {
                    item.SoLuong--;
                    if (item.SoLuong <= 0)
                    {
                        cart.Remove(item);
                    }
                }

                // Lưu lại cũng siêu gọn
                HttpContext.Session.Set("GioHang", cart);
            }

            return RedirectToAction("Index");
        }
    }
}
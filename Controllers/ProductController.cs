using Microsoft.AspNetCore.Mvc;
using QuaMonBa_Store.Data;
using QuaMonBa_Store.ViewComponentModels;
using Microsoft.EntityFrameworkCore;

namespace QuaMonBa_Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly Hshop2023Context _context;
        public ProductController(Hshop2023Context context) => _context = context;

        public IActionResult Index(int? maloai)
        {
            var query = _context.HangHoas.Include(h => h.MaLoaiNavigation).AsQueryable();

            if (maloai.HasValue)
            {
                query = query.Where(h => h.MaLoai == maloai.Value);
            }

            var listHangHoa = query.Select(h => new HangHoaVM
            {
                MaHH = h.MaHh,
                TenHH = h.TenHh,
                DonGia = h.DonGia,
                HinhAnh = h.Hinh,
                MoTaNgan = h.MoTa,
                TenLoai = h.MaLoaiNavigation.TenLoai
            }).ToList();

            return View(listHangHoa);
        }
        [HttpGet]
        public async Task<IActionResult> TimKiemSP(string tuKhoa)
        {

            var query = _context.HangHoas.Include(h => h.MaLoaiNavigation).AsQueryable();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                var listMacDinh = await query.Select(h => new HangHoaVM
                {
                    MaHH = h.MaHh,
                    TenHH = h.TenHh,
                    DonGia = h.DonGia,
                    HinhAnh = h.Hinh,
                    MoTaNgan = h.MoTa,
                    TenLoai = h.MaLoaiNavigation.TenLoai
                }).ToListAsync(); 

                return PartialView("_list_product", listMacDinh);
            }

            ViewBag.TuKhoa = tuKhoa;
            query = query.Where(h => h.TenHh.Contains(tuKhoa));

            var ketQuaSearch = await query.Select(h => new HangHoaVM
            {
                MaHH = h.MaHh,
                TenHH = h.TenHh,
                DonGia = h.DonGia,
                HinhAnh = h.Hinh,
                MoTaNgan = h.MoTa,
                TenLoai = h.MaLoaiNavigation.TenLoai
            }).Take(20).ToListAsync();

            return PartialView("_list_Product_search", ketQuaSearch);
        }
        [HttpGet]
        public async Task<IActionResult> LocTheoGia(double giaTien) 
        {
            var query = _context.HangHoas.Include(h => h.MaLoaiNavigation).AsQueryable();

            // Lọc lấy sản phẩm có giá <= giá tiền khách chọn
            query = query.Where(h => h.DonGia <= giaTien);

            // Biến đổi dữ liệu sang ViewModel
            var ketQuaLoc = await query.Select(h => new HangHoaVM
            {
                MaHH = h.MaHh,
                TenHH = h.TenHh,
                DonGia = h.DonGia,
                HinhAnh = h.Hinh,
                MoTaNgan = h.MoTa,
                TenLoai = h.MaLoaiNavigation.TenLoai
            }).ToListAsync();

            // Bạn có thể dùng lại luôn file _list_product vì nó chung cấu trúc
            return PartialView("_list_product", ketQuaLoc);
        }
    }

}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuaMonBa_Store.Data;
using QuaMonBa_Store.Helpers;
using QuaMonBa_Store.ViewComponentModels;

namespace QuaMonBa_Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly Hshop2023Context _context;
        public ProductController(Hshop2023Context context) => _context = context;

        private IQueryable<HangHoaVM> GetProductQuery()
        {
            return _context.HangHoas.Include(h => h.MaLoaiNavigation)
                .OrderBy(h => h.MaHh)
                .Select(h => new HangHoaVM
                {
                    MaHH = h.MaHh,
                    TenHH = h.TenHh,
                    DonGia = h.DonGia,
                    HinhAnh = h.Hinh,
                    MoTaNgan = h.MoTa,
                    TenLoai = h.MaLoaiNavigation.TenLoai
                });
        }

        public IActionResult Index(int? maloai, int page = 1)
        {
            int pageSize = 9;
            var query = _context.HangHoas.AsQueryable();

            if (maloai.HasValue) query = query.Where(h => h.MaLoai == maloai.Value);

            int totalItems = query.Count();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.MaLoai = maloai;

            var listHangHoa = GetProductQuery()
                .Where(h => !maloai.HasValue || _context.HangHoas.First(x => x.MaHh == h.MaHH).MaLoai == maloai)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            return View(listHangHoa);
        }

        [HttpGet]
        public async Task<IActionResult> TimKiemSP(string tuKhoa, int page = 1)
        {
            int pageSize = 9;
            var query = _context.HangHoas.Where(h => string.IsNullOrEmpty(tuKhoa) || h.TenHh.Contains(tuKhoa));

            int totalItems = await query.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.TuKhoa = tuKhoa;

            var ketQua = await GetProductQuery()
                .Where(h => string.IsNullOrEmpty(tuKhoa) || h.TenHH.Contains(tuKhoa))
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            // Trả về Partial View chứa cả sản phẩm và phân trang
            return PartialView("_list_Product_search", ketQua);
        }

        [HttpGet]
        public async Task<IActionResult> LocTheoGia(double giaTien, int page = 1)
        {
            int pageSize = 9;
            var query = _context.HangHoas.Where(h => h.DonGia <= giaTien);

            int totalItems = await query.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.GiaTien = giaTien;

            var ketQua = await GetProductQuery()
                .Where(h => h.DonGia <= giaTien)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return PartialView("_list_Product", ketQua);
        }
        public IActionResult Edit(int id)
        {
            var product = _context.HangHoas.Find(id);
            if (product == null) return NotFound();
            // Trả về PartialView để nhét vào Modal, không trả về View toàn trang
            return PartialView("_FormInfomationSP", product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(HangHoa product)
        {
            var existingProduct = _context.HangHoas.Find(product.MaHh);
            if (existingProduct == null) return NotFound();

            existingProduct.TenHh = product.TenHh;
            existingProduct.DonGia = product.DonGia;
            // Cập nhật thêm các trường khác nếu cần (Hinh, MaLoai...)

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]    
        public IActionResult Delete(int id) 
        {  
            var product = _context.HangHoas.Find(id);
            if (product == null) return NotFound();
            _context.HangHoas.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
      

        //[Authorize(Roles = "Admin")]
        //public IActionResult Create() 
        //{ 

        //}
    }
}
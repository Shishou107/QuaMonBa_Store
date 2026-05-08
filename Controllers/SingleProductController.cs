using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuaMonBa_Store.Data;

namespace QuaMonBa_Store.Controllers
{
    public class SingleProductController : Controller
    {
        private readonly Hshop2023Context _context;
        public SingleProductController(Hshop2023Context context) => _context = context;

       
        public IActionResult Index(int id)
        {
            var product = _context.HangHoas
                                  .Include(h => h.MaLoaiNavigation)
                                  .FirstOrDefault(h => h.MaHh == id);

            if (product == null) return NotFound();

            return View(product); // Gọi cái file HTML bự chà bá mà bạn vừa làm ở trên
        }
    }
}

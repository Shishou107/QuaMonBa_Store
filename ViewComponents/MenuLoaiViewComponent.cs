using Microsoft.AspNetCore.Mvc;
using QuaMonBa_Store.Data;
using QuaMonBa_Store.ViewComponentModels;

namespace QuaMonBa_Store.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly Hshop2023Context _context;
        public MenuLoaiViewComponent(Hshop2023Context context) => _context = context;

        public IViewComponentResult Invoke()
        {
            var listLoai = _context.Loais.Select(l => new MenuLoaiVM
            {
                MaLoai = l.MaLoai,
                TenLoai = l.TenLoai,
                Soluong = l.HangHoas.Count
            }).OrderBy(p => p.TenLoai).ToList();
            return View(listLoai);
        }
    }
}

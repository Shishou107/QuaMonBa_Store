using Microsoft.AspNetCore.Mvc;

namespace QuaMonBa_Store.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuaMonBa_Store.Models;

namespace QuaMonBa_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contact()
        {
            return View("~/Views/Contact/Index.cshtml");
        }
        public IActionResult About()
        {
            return View("~/Views/About/Index.cshtml");
        }
        public IActionResult Feature()
        {
            return View("~/Views/Feature/Index.cshtml");
        }
        public IActionResult blog()
        {
            return View("~/Views/Blog/Index.cshtml");
        }
        public IActionResult Error_404()
        {
            return View("~/Views/_eror_404/Index.cshtml");
        }
        public IActionResult Testimonial()
        {
            return View("~/Views/testimonial/Index.cshtml");
        }
        public IActionResult How_to_used()
        {
            return View("~/Views/How_to_used/Index.cshtml");
        }
    }
}

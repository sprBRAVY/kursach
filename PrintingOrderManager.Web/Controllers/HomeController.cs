// PrintingOrderManager.Web/Controllers/HomeController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PrintingOrderManager.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [Authorize]
        public IActionResult Dashboard() => View();

        [Authorize]
        public IActionResult Profile() => View();

        public IActionResult AccessDenied() => View();

        public IActionResult Privacy() => View();
    }
}
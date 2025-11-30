// PrintingOrderManager.Web/Controllers/HomeController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PrintingOrderManager.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Главная страница для НЕ авторизованных: выбор входа или регистрации
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            // Главная страница для авторизованных
            return View();
        }

        public IActionResult AccessDenied() => View();
    }
}
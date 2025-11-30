// PrintingOrderManager.Web/Controllers/AccountController.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PrintingOrderManager.Application.Services;
using PrintingOrderManager.Core.DTOs;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        if (await _authService.IsEmailTakenAsync(dto.Email))
        {
            ModelState.AddModelError("Email", "Email уже используется");
            return View(dto);
        }

        await _authService.RegisterAsync(dto);
        return RedirectToAction("Login");
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Заполните все поля");
            return View();
        }

        var user = await _authService.AuthenticateAsync(email, password);
        if (user == null)
        {
            ModelState.AddModelError("", "Неверный email или пароль");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        return RedirectToAction("Dashboard", "Home"); // ← ИЗМЕНЕНО: не Index, а Dashboard
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home"); // ← Возврат на главную (выбор входа/регистрации)
    }
}
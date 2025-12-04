using DMSUI.Entities.DTOs.Auth;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DMSUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        public IActionResult Login()
        {
            var token = Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new LoginRequestDTO
            {
                Email = model.Email,
                Password = model.Password
            };

            var result = await _authManager.LoginAsync(request);

            if (result == null || string.IsNullOrEmpty(result.AccessToken))
            {
                ViewBag.Error = "Email veya şifre hatalı";
                return View(model);
            }

            Response.Cookies.Append("access_token", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,   
                SameSite = SameSiteMode.Lax
            });

            Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });

            return RedirectToAction("Index", "Dashboard");

        }
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");
            return RedirectToAction("Login", "Auth");
        }
    }
}

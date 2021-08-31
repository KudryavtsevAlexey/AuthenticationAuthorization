using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationAuthentication.Data;
using AuthorizationAuthentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationAuthentication.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize(Policy ="Administrator")]
        public IActionResult Administrator()
        {
            return View();
        }

        [Authorize(Policy ="Manager")]
        public IActionResult Manager()
        {
            return View();
        }


        [AllowAnonymous, HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _dbContext.Users.SingleOrDefaultAsync(u =>
                u.UserName == model.UserName && u.Password == model.Password);

            if (user!=null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };

                var claimIdentity = new ClaimsIdentity(claims, "Cookie");
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                await HttpContext.SignInAsync("Cookie", claimPrincipal);
                return Redirect(model.ReturnUrl);
            }
            ModelState.AddModelError("","User not found");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookie");
            return RedirectToAction("Index", "Home");
        }
    }
}

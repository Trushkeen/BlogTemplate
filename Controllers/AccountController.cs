using BlogTemplate.Core.Models;
using BlogTemplate.Core.ViewModels;
using BlogTemplate.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogTemplate.Core.Controllers
{
    public class AccountController : Controller
    {
        private BlogContext db;
        private CurrentUserService currentUser;

        public AccountController(BlogContext dbUsers, CurrentUserService currentUser)
        {
            db = dbUsers;
            this.currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> ShowProfile()
        {
            return View("Profile", await currentUser.GetCurrentUserAsync());
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if(user != null)
                {
                    await Authenticate(user.Login);

                    return RedirectToAction("LatestPosts", "Post");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    db.Users.Add(new User { Login = model.Login, Password = model.Password });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Login);

                    return RedirectToAction("LatestPosts", "Post");
                }
            }
            return View(model);
        }

        private async Task Authenticate(string username)
        {
            var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, username) };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}

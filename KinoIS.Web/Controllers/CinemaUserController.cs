
using KinoIs.Repository;
using KinoIS.Domain.Models;
using KinoIS.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KinoIS.Web.Controllers
{
    public class CinemaUserController : Controller
    {
        private readonly ShoppingCartService shoppingCartService;
        private DbSet<CinemaUser> entities;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;
        public CinemaUserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, 
            ApplicationDbContext context, ShoppingCartService shoppingCartService)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            entities = context.Set<CinemaUser>();
            this.shoppingCartService = shoppingCartService; 
        }

        public IActionResult Register()
        {
            CinemaUser model = new CinemaUser();
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(CinemaUser request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new CinemaUser
                    {
                        Name = request.Name,
                        Surname = request.Surname,
                        UserName = request.Email,
                        Role = "User",
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = request.PhoneNumber,
                    }; 
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {

                        ShoppingCart shoppingCart = this.shoppingCartService.create(user.Id);
                        user.UserCart = shoppingCart;

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(request);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }

            return View(request);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            CinemaUser model = new CinemaUser();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(CinemaUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet");
                    return View(model);

                }
                if (await userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid credentials");
                    return View(model);

                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    return View(model);
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult GiveRole()
        {
            List<CinemaUser> users = context.users.ToList();
            return View(users);
        }
        public IActionResult GiveRoleTo(Guid id)
        {
            string normalizedId = id.ToString();
            CinemaUser user = entities.SingleOrDefault(x => x.Id.Equals(normalizedId));
            user.Role = "Admin";
            context.SaveChanges();
            return RedirectToAction("GiveRole", "CinemaUser");
        }
    }
}

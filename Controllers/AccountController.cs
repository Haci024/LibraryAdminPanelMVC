using LibraryAdminPanel.DAL;
using LibraryAdminPanel.Models;
using LibraryAdminPanel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LibraryAdminPanel.Controllers
{

    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            AppDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            ViewBag.IsExistAdmin = false;
            HasAdmins hasAdmin = await _db.HasAdmin.FirstOrDefaultAsync();
            if (hasAdmin.HasAdmin)
            {
                ViewBag.IsExistAdmin = true;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVm)
        {
            ViewBag.IsExistAdmin = false;
            HasAdmins hasAdmin = await _db.HasAdmin.FirstOrDefaultAsync();
            if (hasAdmin.HasAdmin)
            {
                ViewBag.IsExistAdmin = true;
            }
            AppUser appUser = await _userManager.FindByNameAsync(loginVm.Username);
            if (appUser == null)
            {
                ModelState.AddModelError("Username", "İstifadəçi adı yalnışdır");
             
                return View();
            }

            if (appUser.IsDeactive)
            {
                ModelState.AddModelError("", "İstifadəçi blok olunub");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVm.Password, loginVm.RememberMe,true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Şifrəni 5 dəfə səhv yazdığınız üçün müvəqqəti olaraq bloklandı");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Şifrə düzgün deyil");
                return View();
            }
            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> CreateAdmin()
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            await CreateRoles();
            AppUser appUser = await _userManager.FindByNameAsync("Admin");
            if (appUser != null)
            {
                return NotFound();
            }

            AppUser newUser = new AppUser
            {

                FullName = "Admin24",
                UserName = "Admin24",
                Email = "Admin@admin.com"

            };
            await _userManager.CreateAsync(newUser, "Admin1234");
            await _userManager.AddToRoleAsync(newUser, "SuperAdmin");
            HasAdmins hasAdmin = await _db.HasAdmin.FirstOrDefaultAsync();
            hasAdmin.HasAdmin = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Login");
        }
        public async Task CreateRoles()
        {
            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
            }
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }
        }
        #region MyRegion
        public IActionResult ForgotPasswordForEmail()
        {
            return View();
        }

        public async Task<IActionResult> ForgotPasswordVerifyAsync(string email)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordVerifyAsync(string email, ForgotPasswordVerify forgotPasswordVerify)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null)
            {
                return View("Error");
            }



            return RedirectToAction("ForgotPassword", new { email });
        }

        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (email == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email, ResetPasswordVM resetPassword)
        {
            if (email == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, resetPassword.Password);
            if (!identityResult.Succeeded)
            {
                ModelState.AddModelError("", "Şifrə min 6 simvoldan ibarət olmalıdır");
                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd kiçik hərf olmalıdır");
                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd böyük hərf olmalıdır");
                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd rəqəm hərf olmalıdır");
                return View();
            }
            return RedirectToAction("Login");
        }
        #endregion
        public async Task<IActionResult> MyProfileAsync()
        {
           
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            
            //Us role = await _roleManager.FindByIdAsync();
            //role = ViewBag.Role;
            //if (role==null)
            //{
            //    return View("Error");
            //}
            if (appUser == null)
            {
                return View("Error");
            }
            return View(appUser);
        }
    }

}

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
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext db,
            SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    //Surname = user.Surname,
                    UserName = user.UserName,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };
                userVMs.Add(userVM);
            }
            return View(userVMs);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUserForEmail = await _userManager.FindByEmailAsync(userVM.Email);
            AppUser appUserForName = await _userManager.FindByNameAsync(userVM.UserName);
            if (appUserForEmail != null)
            {
                ModelState.AddModelError("", "Bu emaildə artıq hesab var");
                return View();
            }
            if (appUserForName != null)
            {
                ModelState.AddModelError("", "Bu istifadəçi adında artıq hesab var");
                return View();
            }
            AppUser newUser = new AppUser
            {
                //Name = userVM.Name,
                FullName = userVM.FullName,
                UserName = userVM.UserName,
                Email = userVM.Email

            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, userVM.Password);
            if (!identityResult.Succeeded)
            {
                ModelState.AddModelError("", "Şifrə min 6 simvoldan ibarət olmalıdır");
                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd kiçik hərf olmalıdır");
                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd böyük hərf olmalıdır");
                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd rəqəm hərf olmalıdır");
                return View();
            }
            await _userManager.AddToRoleAsync(newUser, "Admin");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UpdateUserVM dbUserVM = new UpdateUserVM
            {
               
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                //Surname = user.Surname,
            };
            return View(dbUserVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateUserVM userVM)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UpdateUserVM dbUserVM = new UpdateUserVM
            {
                Email = user.Email,
                FullName = user.FullName,
                //Name = user.UserName,
                //Surname=user.Surname,
                UserName =user.UserName,
            };
            if (!ModelState.IsValid)
            {
                return View(dbUserVM);
            }
            AppUser appUserForEmail = await _userManager.FindByEmailAsync(userVM.Email);
            AppUser appUserForName = await _userManager.FindByNameAsync(userVM.UserName);
            if (appUserForEmail != null)
            {
                if (appUserForEmail.Id != id)
                {
                    ModelState.AddModelError("", "Bu emaildə artıq hesab var");
                    return View(dbUserVM);
                }

            }
            if (appUserForName != null)
            {
                if (appUserForName.Id != id)
                {
                    ModelState.AddModelError("", "Bu emaildə artıq hesab var");
                    return View(dbUserVM);
                }
            }
            user.Email = userVM.Email;
            user.FullName = userVM.FullName;
            //user.Name = userVM.Name;
            //user.Surname = userVM.Surname;
            user.UserName = userVM.UserName;
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Activity(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.IsDeactive)
            {
                user.IsDeactive = false;
            }
            else
            {
                user.IsDeactive = true;
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> UpdatePassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(string id, ResetPasswordVM resetPassword)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, resetPassword.Password);
            if (!identityResult.Succeeded)
            {
                ModelState.AddModelError("", "Şifrə min 6 simboldan ibarət olmalıdır");
                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd kiçik hərf olmalıdır");
                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd böyük hərf olmalıdır");
                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd rəqəm hərf olmalıdır");
                return View();
            }
            return RedirectToAction("Index");
        }
     
    }
}

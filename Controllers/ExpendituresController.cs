using LibraryAdminPanel.DAL;
using LibraryAdminPanel.Models;
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
    public class ExpendituresController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public ExpendituresController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<Expenditure> expenditures = await _db.Expenditures.ToListAsync();
            return View(expenditures);
        }//Include(x => x.AppUser).

        #region Create

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expenditure expenditure)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            kassa.LastModifiedBy = user.FullName;
            kassa.Balance -= expenditure.Money;
            kassa.LastModifiedMoney = expenditure.Money- expenditure.Money- expenditure.Money;
            kassa.LastModified = expenditure.For;
            kassa.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            expenditure.StartTime = DateTime.UtcNow.AddHours(4);
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            //expenditure.AppUserId = appUser.Id;
            await _db.Expenditures.AddAsync(expenditure);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion



    }
}

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
    public class PaidedSalariesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public PaidedSalariesController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<PaidedSalary> paidedSalaries = await _db.PaidedSalaries.Include(x=>x.Employee).ToListAsync();
            return View(paidedSalaries);
        }//Include(x=>x.AppUser).

        #region Create

        public async Task<IActionResult> Create()
        {
            ViewBag.Employers = await _db.Employers.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaidedSalary paidedSalary, int empId)
        {
            ViewBag.Employers = await _db.Employers.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            Employee employee = await _db.Employers.FirstOrDefaultAsync(x=>x.Id==empId);
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            kassa.LastModifiedBy = user.FullName;
            kassa.Balance -= (float)employee.Salary;
            kassa.LastModifiedMoney = (float)employee.Salary- (float)employee.Salary- (float)employee.Salary;

            kassa.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            if (employee.IsMale)
            {
                kassa.LastModified = $"{employee.Name} {employee.Surname} {employee.FatherName} oğluna maaş ödənildi";
            }
            else
            {
                kassa.LastModified = $"{employee.Name} {employee.Surname} {employee.FatherName} qızına maaş ödənildi";
            }
            paidedSalary.EmployeeId = empId;
            paidedSalary.LastModifiedTime = DateTime.UtcNow.AddHours(4);
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            //paidedSalary.AppUserId = appUser.Id;
            paidedSalary.Money = (float)employee.Salary;
            await _db.PaidedSalaries.AddAsync(paidedSalary);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion



    }
}

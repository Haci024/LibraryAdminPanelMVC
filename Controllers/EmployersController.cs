using Employers.Helpers;
using LibraryAdminPanel.DAL;
using LibraryAdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EmployersController : Controller
    {
        private readonly AppDbContext _db;
        public EmployersController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _db.Employers.Include(x => x.EmployeePositions).ThenInclude(x => x.Position).ToListAsync();

            return View(employees);
        }
        public async Task<IActionResult> Create()
        {
            List<Position> positions = new List<Position>();
            positions = await _db.Positions.ToListAsync();
            ViewBag.Positons = positions;
            ViewBag.PosCount = positions.Count();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, string[] posIds, bool isMale)
        {

            ViewBag.Positons = await _db.Positions.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            List<EmployeePosition> employeePositions = new List<EmployeePosition>();

            foreach (string id in posIds)
            {
                employeePositions.Add(new EmployeePosition
                { PositionId = int.Parse(id) });
            }
            employee.EmployeePositions = employeePositions;
            employee.IsMale = isMale;
            employee.StartTime = DateTime.UtcNow.AddHours(4);
            await _db.Employers.AddAsync(employee);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> SendSmS(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Employee employee = await _db.Employers.FirstOrDefaultAsync(x => x.Id == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.Fullname = employee.Name + " " + employee.Surname;
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SendSmS(int? id,MessageVM messageVm)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Employee employee = await _db.Employers.FirstOrDefaultAsync(x => x.Id == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.Fullname = employee.Name + " " + employee.Surname;
        //    var response = await Helper.SendSmsAsync(messageVm.Message, "+994" + employee.Phone);
        //    return RedirectToAction("Index");
        //}
        public IActionResult SendAllSmS()
        {
           
            
            return View();
        }
   
        public async Task<IActionResult> SendEmail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = await _db.Employers.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.Fullname = employee.Name + " " + employee.Surname;
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SendEmail(int? id, MessageVM messageVm)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Employee employee = await _db.Employers.FirstOrDefaultAsync(x => x.Id == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    await Helper.SendEmailMessageAsync("İşçilər sistemi", messageVm.Message, employee.Email);
        //    return RedirectToAction("Index");
        //}
        //public IActionResult SendAllEmail()
        //{
            
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SendAllEmail( MessageVM messageVm)
        //{
        //    List<Employee> employees = await _db.Employers.ToListAsync();
        //    foreach (var employee in employees)
        //    {
        //        await Helper.SendEmailMessageAsync("İşçilər sistemi", messageVm.Message, employee.Email);
        //    }
            
        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = await _db.Employers.FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            if (employee.IsDeactive)
            {
                employee.IsDeactive = false;
            }
            else
            {
                employee.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = await _db.Employers.Include(x=>x.EmployeePositions).FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.Positons = await _db.Positions.ToListAsync();
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Employee employee, string[] posIds, bool isMale)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee dbEmployer = await _db.Employers.Include(x => x.EmployeePositions).FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployer == null)
            {
                return NotFound();
            }
            ViewBag.Positons = await _db.Positions.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(dbEmployer);
            }

            List<EmployeePosition> employeePositions = new List<EmployeePosition>();

            foreach (string posId in posIds)
            {
                employeePositions.Add(new EmployeePosition { PositionId = int.Parse(posId) });
            }
            employee.EmployeePositions = employeePositions;
            employee.IsMale = isMale;
            dbEmployer.DateOfBirth = employee.DateOfBirth;
            dbEmployer.Email = employee.Email;
            dbEmployer.Name = employee.Name;
            dbEmployer.Surname = employee.Surname;
            dbEmployer.FatherName = employee.FatherName;
            dbEmployer.Phone = employee.Phone;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = await _db.Employers.Include(x => x.EmployeePositions).ThenInclude(x=>x.Position).Include(x=>x.PaidedSalaries).FirstOrDefaultAsync(x => x.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.Positons = await _db.Positions.ToListAsync();
            return View(employee);
        }
    }
}

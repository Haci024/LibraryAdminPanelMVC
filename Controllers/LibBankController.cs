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
    public class LibBankController : Controller
    {
        private readonly AppDbContext _db;
        public LibBankController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            Kassa kassa = await _db.Kassas.FirstOrDefaultAsync();
            return View(kassa);
        }
    }
}

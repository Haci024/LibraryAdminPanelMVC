
using LibraryAdminPanel.DAL;
using LibraryAdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employers.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PositionsController : Controller
    {
        private readonly AppDbContext _db;
        public PositionsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Position> position = await _db.Positions.ToListAsync();
            return View(position);
        }

        #region Create

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Positions.AnyAsync(x => x.Name == position.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda vəzifə artıq mövcuddur");
                return View();
            }
            await _db.Positions.AddAsync(position);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion


        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPosition == null)
            {
                return NotFound();
            }
            return View(dbPosition);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Position position)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPosition == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(dbPosition);
            }
            bool isExist = await _db.Positions.AnyAsync(x => x.Name == position.Name&& x.Id!=id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda vəzifə artıq mövcüddür");
                return View(dbPosition);
            }
            dbPosition.Name = position.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position position = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (position == null)
            {
                return NotFound();
            }
            if (!position.IsDeactive)
            {
                position.IsDeactive = true;
            }
            else
            {
                position.IsDeactive = false;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

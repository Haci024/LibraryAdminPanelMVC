using LibraryAdminPanel.DAL;
using LibraryAdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAdminPanel.Controllers
{

    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CategoryController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public CategoryController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;
        }

        #endregion
        #region Index
        public IActionResult Index()
        {

            List<Categories> Categories = _db.Categories.Include(x=>x.Books).ToList();

            return View(Categories);
        }
     
        #endregion
        #region Create
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categories Categories)
        {
            bool IsExist = _db.Categories.Any(x => x.Name == Categories.Name);
            if (IsExist == true)
            {
                ModelState.AddModelError("Name", "Bu departament artıq mövcuddur!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            await _db.Categories.AddAsync(Categories);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Categories Categories = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (Categories == null)
            {
                return View("Error");
            }
            return View(Categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Categories changedCategories)
        {
            if (id == null)
            {
                return View("Error");
            }
            Categories dbCategories = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCategories == null)
            {
                return View("Error");
            }
            bool IsExist = _db.Categories.Any(x => x.Name == changedCategories.Name && x.Id != id);

            if (IsExist == true)
            {
                ModelState.AddModelError("Name", "Bu departament artıq mövcuddur!");

                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
       
            dbCategories.Name = changedCategories.Name;
            dbCategories.Description = changedCategories.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Categories Categories = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (Categories == null)
            {
                return View("Error");
            }
            if (Categories.IsDeactive)
            {
                Categories.IsDeactive = false;
            }
            else
            {
                Categories.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Detail
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Categories Categories = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (Categories == null)
            {
                return View("Error");
            }
            return View(Categories);
        }
        #endregion
        #region Books

        public async Task<IActionResult> Books(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }


            Categories Categories = await _db.Categories.Include(x=>x.Books).FirstOrDefaultAsync(x => x.Id == id);

            if (Categories == null)
            {
                return View("Error");
            }

            return View(Categories);
        }
        #endregion
     

    }
}

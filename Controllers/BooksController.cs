using LibraryAdminPanel.DAL;
using LibraryAdminPanel.Helpers;
using LibraryAdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class BooksController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public BooksController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;

        }
        #endregion
        #region Index
        public IActionResult Index()
        {

           
            List<Books> Books = _db.Books.Include(x=>x.Categories).Include(x => x.BookAuthors).ThenInclude(x=>x.Authors).ToList();
            ViewBag.Category = _db.Categories.Include(x => x.Books).ToList();
            return View(Books);
        }

        //public async Task<IActionResult> LoadSelectCategoryPartial(int? mainId)
        //{
        //    if (mainId == null)
        //    {
        //        return View("Error");
        //    }
        //    List<Books> Books = await _db.Books.Include(x => x.Categories).Where(x => x.CategoryId == mainId).ToListAsync();
          
        //    return PartialView("_LoadSelectCategoryPartial", Books);
        //}
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Books Books = await _db.Books.Include(x => x.BookAuthors).ThenInclude(x => x.Authors).FirstOrDefaultAsync(x => x.Id == id);
            if (Books == null)
            {
                return View("Error");
            }
            if (Books.IsDeactive)
            {
                Books.IsDeactive = false;
            }
            else
            {
                Books.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.Include(x=>x.Books).Where(x => x.IsDeactive == false).ToListAsync();
            ViewBag.Authors = await _db.Author.Include(x => x.BookAuthors).ThenInclude(x => x.Books).Where(x => x.IsDeactive == false).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Books newBooks, string[] AuthorsId, int CategoryId)
        {

            ViewBag.Categories = await _db.Categories.Include(x => x.Books).Where(x => x.IsDeactive == false).ToListAsync();
            ViewBag.Authors = await _db.Author.Include(x => x.BookAuthors).ThenInclude(x => x.Books).Where(x => x.IsDeactive == false).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
          
            if (newBooks.BooksPhoto == null)
            {
                ModelState.AddModelError("BooksPhoto", "Şəkil seçin!");
                return View();
            }
            if (!newBooks.BooksPhoto.IsImage())
            {
                ModelState.AddModelError("BooksPhoto", "Şəkil  faylı seçin!");

                return View();
            }
            if (newBooks.BooksPhoto.IsMore5MB())
            {
                ModelState.AddModelError("BooksPhoto", "Şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            newBooks.BooksImage = await newBooks.BooksPhoto.SaveImageAsync(path);
            List<BookAuthors> bookAuthors = new List<BookAuthors>();

            foreach (string id in AuthorsId)
            {
                bookAuthors.Add(new BookAuthors
                {
                    AuthorsId = int.Parse(id)
                });
            }
            newBooks.BookAuthors = bookAuthors;
            newBooks.CategoriesId = CategoryId;
            newBooks.CreateTime = DateTime.UtcNow.AddHours(4);
            await _db.Books.AddAsync(newBooks);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Books Books = await _db.Books.Include(x => x.Categories).Include(x => x.BookAuthors).ThenInclude(x => x.Authors).FirstOrDefaultAsync(x => x.Id == id);
            if (Books == null)
            {
                return View("Error");
            }
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Authors = await _db.Author.ToListAsync();
            return View(Books);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Books changeBooks, int CategoryId, string[] AuthorsId)
        {
            if (id == null)
            {
                return View("Error");
            }
            Books dbBooks = await _db.Books.Include(x => x.Categories).Include(x => x.BookAuthors).ThenInclude(x => x.Authors).FirstOrDefaultAsync(x => x.Id == id);
            if (dbBooks == null)
            {
                return View("Error");
            }
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Authors= await _db.Author.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(dbBooks);
            }



            List<BookAuthors> bookAuthors = new List<BookAuthors>();

            foreach (string Id in AuthorsId)
            {
                bookAuthors.Add(new BookAuthors
                {
                    AuthorsId = int.Parse(Id)
                });
            }
           
            if (changeBooks.BooksPhoto != null)
            {

                if (!changeBooks.BooksPhoto.IsImage())
                {
                    ModelState.AddModelError("BooksPhoto", "Şəkil faylı seçin!");

                    return View();
                }

                if (changeBooks.BooksPhoto.IsMore5MB())
                {
                    ModelState.AddModelError("BooksPhoto", "Şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbBooks.BooksImage = await changeBooks.BooksPhoto.SaveImageAsync(path);
            }
            dbBooks.BookAuthors = bookAuthors;
            dbBooks.CategoriesId = CategoryId;
            dbBooks.Name = changeBooks.Name;
            dbBooks.CreateTime = changeBooks.CreateTime;
            dbBooks.Description = changeBooks.Description;
            dbBooks.Name = changeBooks.Name;

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion
        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Books Books = _db.Books.Include(x => x.Categories).Include(x => x.BookAuthors).ThenInclude(x => x.Authors).FirstOrDefault(x => x.Id == id);
            ViewBag.Categories = await _db.Categories.ToListAsync();
            if (Books == null)
            {

                return View("Error");
            }
            return View(Books);
        }
        #endregion
        //#region Patients
        //public async Task<IActionResult> Patients(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View("Error");
        //    }
        //    Books Books = await _db.Books.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
        //    ViewBag.Patients = await _db.Patients.Include(x => x.Reports).ThenInclude(x => x.Bookss).ToListAsync();
        //    if (Books == null)
        //    {
        //        return View("Error");
        //    }
        //    return View(Books);
        //}
        //public async Task<IActionResult> SendMail(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View("Error");
        //    }

        //    Books Books = await _db.Books.FirstOrDefaultAsync(x => x.Id == id);
        //    string msg = " ";
        //    MailMessage mailMessage = new MailMessage();


        //    try
        //    {
        //        mailMessage.To = Books.Email;

        //        await EmailUtil.SendEmailMessageAsync(Books.Name + " " + Books.Surname + " " + mailMessage.Subject, mailMessage.Body, Books.Email);

        //    }
        //    catch (Exception e)
        //    {

        //        msg = "Mail göndərilə bilmədi!";
        //    }
        //    ViewBag.Mgs = msg;



        //    return View(mailMessage);

        //}
        //[HttpPost]
        //public async Task<IActionResult> SendMail(int? id, MailMessage mailMessage)
        //{
        //    if (id == null)
        //    {
        //        return View("Error");
        //    }
        //    Books Books = await _db.Books.FirstOrDefaultAsync(x => x.Id == id);

        //    string msg = " ";

        //    if (mailMessage.Subject == null)
        //    {

        //        ModelState.AddModelError("Subject", " ");
        //        return View();
        //    }
        //    try
        //    {
        //        mailMessage.To = Books.Email;
        //        if (Books.Type == "Kişi")
        //        {
        //            await EmailUtil.SendEmailMessageAsync("Cənab Dk." + Books.Name + " " + Books.Surname + " " + mailMessage.Subject, mailMessage.Body, Books.Email);
        //        }
        //        else
        //        {
        //            await EmailUtil.SendEmailMessageAsync("Xanım Dk." + Books.Name + " " + Books.Surname + " " + mailMessage.Subject, mailMessage.Body, Books.Email);
        //        }
        //        msg = "Mesaj göndərildi!";

        //    }
        //    catch (Exception e)
        //    {

        //        msg = "Mail göndərilə bilmədi!";
        //    }
        //    ViewBag.Mgs = msg;
        //    return View();

        //}
        //#endregion
    }

}





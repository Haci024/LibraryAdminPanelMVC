using LibraryAdminPanel.Helpers;
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
    public class AuthorsController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public AuthorsController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;

        }
        #endregion
        #region Index
        public IActionResult Index()
        {


            List<Authors> Authors = _db.Author.ToList();

            return View(Authors);
        }

     
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Authors Author = await _db.Author.FirstOrDefaultAsync(x => x.Id == id);
            if (Author == null)
            {
                return View("Error");
            }
            if (Author.IsDeactive)
            {
                Author.IsDeactive = false;
            }
            else
            {
                Author.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Create
        public  IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Authors newAuthor)
        {

            if (!ModelState.IsValid)
            {

                return View();
            };
            if (newAuthor.AuthorPhoto == null)
            {
                ModelState.AddModelError("AuthorPhoto", "Şəkil seçin!");
                return View();
            }
            if (!newAuthor.AuthorPhoto.IsImage())
            {
                ModelState.AddModelError("AuthorPhoto", "Şəkil  faylı seçin!");

                return View();
            }
            if (newAuthor.AuthorPhoto.IsMore5MB())
            {
                ModelState.AddModelError("AuthorPhoto", "Şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            newAuthor.AuthorImage = await newAuthor.AuthorPhoto.SaveImageAsync(path);
        



            await _db.Author.AddAsync(newAuthor);
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
            Authors Author = await _db.Author.FirstOrDefaultAsync(x => x.Id == id);
            if (Author == null)
            {
                return View("Error");
            }


            return View(Author);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Authors changeAuthor)
        {
            if (id == null)
            {
                return View("Error");
            }
            Authors dbAuthor = await _db.Author.FirstOrDefaultAsync(x => x.Id == id);
            if (dbAuthor == null)
            {
                return View("Error");
            }

            if (!ModelState.IsValid)
            {
                return View(dbAuthor);
            }
            if (changeAuthor.AuthorPhoto != null)
            {

                if (!changeAuthor.AuthorPhoto.IsImage())
                {
                    ModelState.AddModelError("AuthorPhoto", "Şəkil faylı seçin!");

                    return View();
                }

                if (changeAuthor.AuthorPhoto.IsMore5MB())
                {
                    ModelState.AddModelError("AuthorPhoto", "Şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbAuthor.AuthorImage = await changeAuthor.AuthorPhoto.SaveImageAsync(path);
            }
            dbAuthor.Name = changeAuthor.Name;
            dbAuthor.Surname = changeAuthor.Surname;
            dbAuthor.BirthDate = changeAuthor.BirthDate;
            dbAuthor.Description = changeAuthor.Description;
           


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
            Authors Author = await _db.Author.Include(x => x.BookAuthors).ThenInclude(x => x.Authors).FirstOrDefaultAsync(x => x.Id == id);

            if (Author == null)
            {

                return View("Error");
            }
            return View(Author);
        }
        #endregion
        //#region Patients
        //public async Task<IActionResult> Patients(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View("Error");
        //    }
        //    Author Author = await _db.Author.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
        //    ViewBag.Patients = await _db.Patients.Include(x => x.Reports).ThenInclude(x => x.Authors).ToListAsync();
        //    if (Author == null)
        //    {
        //        return View("Error");
        //    }
        //    return View(Author);
        //}
        //public async Task<IActionResult> SendMail(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View("Error");
        //    }

        //    Author Author = await _db.Author.FirstOrDefaultAsync(x => x.Id == id);
        //    string msg = " ";
        //    MailMessage mailMessage = new MailMessage();


        //    try
        //    {
        //        mailMessage.To = Author.Email;

        //        await EmailUtil.SendEmailMessageAsync(Author.Name + " " + Author.Surname + " " + mailMessage.Subject, mailMessage.Body, Author.Email);

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
        //    Author Author = await _db.Author.FirstOrDefaultAsync(x => x.Id == id);

        //    string msg = " ";

        //    if (mailMessage.Subject == null)
        //    {

        //        ModelState.AddModelError("Subject", " ");
        //        return View();
        //    }
        //    try
        //    {
        //        mailMessage.To = Author.Email;
        //        if (Author.Type == "Kişi")
        //        {
        //            await EmailUtil.SendEmailMessageAsync("Cənab Dk." + Author.Name + " " + Author.Surname + " " + mailMessage.Subject, mailMessage.Body, Author.Email);
        //        }
        //        else
        //        {
        //            await EmailUtil.SendEmailMessageAsync("Xanım Dk." + Author.Name + " " + Author.Surname + " " + mailMessage.Subject, mailMessage.Body, Author.Email);
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

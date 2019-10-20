using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using ImageSharingWithModel.DAL;
using ImageSharingWithModel.Models;
using Newtonsoft.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace ImageSharingWithModel.Controllers
{
    public class ImagesController : BaseController
    {
        private readonly ApplicationDbContext db;

        private readonly IHostingEnvironment hostingEnvironment;

        private readonly ILogger<ImagesController> logger;

        // Dependency injection
        public ImagesController(ApplicationDbContext db, IHostingEnvironment environment, ILogger<ImagesController> logger)
        {
            this.db = db;
            this.hostingEnvironment = environment;
            this.logger = logger;
        }

        protected void mkDirectories()
        {
            var dataDir = Path.Combine(hostingEnvironment.WebRootPath,
               "data", "images");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
        }

        protected string imageDataFile(int id)
        {
            return Path.Combine(
               hostingEnvironment.WebRootPath,
               "data", "images", "img-" + id + ".jpg");
        }

        public static string imageContextPath(int id)
        {
            return "data/images/img-" + id + ".jpg";
        }


        [HttpGet]
        public ActionResult Upload()
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            ViewBag.Message = "";
            ImageView imageView = new ImageView();
            imageView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View(imageView);
        }

        [HttpPost]
        public ActionResult Upload(ImageView imageView)
        {
            CheckAda();


            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            TryUpdateModelAsync(imageView);

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please correct the errors in the form!";
                imageView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
                return View();
            }

            User user = db.Users.SingleOrDefault(u => u.Username.Equals(Username));
            if (user == null)
            {
                ViewBag.Message = "No such Username registered!";
                return View();
            }

            if (imageView.ImageFile == null || imageView.ImageFile.Length <= 0)
            {
                ViewBag.Message = "No image file specified!";
                imageView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
                return View(imageView);
            }

            Image image = new Image();
            // TODO save image metadata in the database 
            image.Caption = imageView.Caption;
            image.TagId = imageView.TagId;
            image.Description = imageView.Description;
            image.DateTaken = imageView.DateTaken;
            image.UserId = user.Id;

            db.Entry(image).State = EntityState.Added;
            db.SaveChanges();
            // end TODO

            mkDirectories();

            // TODO save image file on disk
            imageView.ImageFile.CopyToAsync(new FileStream(imageDataFile(image.Id), FileMode.Create));           
            // end TODO

            return RedirectToAction("Details", new { Id = image.Id });
        }

        [HttpGet]
        public ActionResult Query()
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            ViewBag.Message = "";
            return View();
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            Image image = db.Images.Find(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "Details:" + Id });
            }

            ImageView imageView = new ImageView();
            imageView.Id = image.Id;
            imageView.Caption = image.Caption;
            imageView.Description = image.Description;
            imageView.DateTaken = image.DateTaken;
            /*
             * Explicit loading of related entities
             */
            var imageEntry = db.Entry(image);
            imageEntry.Reference(i => i.Tag).Load();
            imageEntry.Reference(i => i.User).Load();

            imageView.TagName = image.Tag.Name;
            imageView.Username = image.User.Username;
            return View(imageView);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            Image image = db.Images.Find(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "EditNotFound" });
            }

            String Username = GetLoggedInUser();
            db.Entry(image).Reference(im => im.User).Load();  // Explicit load of user
            if (!image.User.Username.Equals(Username))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "EditNotAuth" });
            }

            ViewBag.Message = "";

            ImageView imageView = new ImageView();
            imageView.Tags = new SelectList(db.Tags, "Id", "Name", image.TagId);
            imageView.Id = image.Id;
            imageView.TagId = image.TagId;
            imageView.Caption = image.Caption;
            imageView.Description = image.Description;
            imageView.DateTaken = image.DateTaken;

            return View("Edit", imageView);
        }

        [HttpPost]
        public ActionResult DoEdit(int Id, ImageView imageView)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please correct the errors on the page";
                imageView.Id = Id;
                imageView.Tags = new SelectList(db.Tags, "Id", "Name", imageView.TagId);
                return View("Edit", imageView);
            }

            logger.LogDebug("Saving changes to image " + Id);
            Image image = db.Images.Find(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "EditNotFound" });
            }

            db.Entry(image).Reference(im => im.User).Load();  // Explicit load of user
            if (!image.User.Username.Equals(Username))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "EditNotAuth" });
            }
            image.TagId = imageView.TagId;
            image.Caption = imageView.Caption;
            image.Description = imageView.Description;
            image.DateTaken = imageView.DateTaken;
            db.Entry(image).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { Id = Id });
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            Image image = db.Images.Find(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "Delete" });
            }

            db.Entry(image).Reference(im => im.User).Load();  // Explicit load of user
            if (!image.User.Username.Equals(Username))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "DeleteNotAuth" });
            }

            ImageView imageView = new ImageView();
            // TODO display image and metadata for confirmation
            var imageEntry = db.Entry(image);
            imageEntry.Reference(i => i.Tag).Load();
            imageEntry.Reference(i => i.User).Load();
            imageView.Id = image.Id;
            imageView.TagName = image.Tag.Name;
            imageView.Caption = image.Caption;
            imageView.Description = image.Description;
            imageView.DateTaken = image.DateTaken;
            imageView.Username = image.User.Username;
            return View(imageView);
        }

        [HttpPost]
        public ActionResult DoDelete(int Id)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            Image image = db.Images.Find(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "DeleteNotFound" });
            }

            db.Entry(image).Reference(im => im.User).Load();  // Explicit load of user
            if (!image.User.Username.Equals(Username))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "DeleteNotAuth" });
            }

            // TODO delete from the database
            db.Entry(image).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public ActionResult ListAll()
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            IEnumerable<Image> images = db.Images.Include(im => im.User).Include(im => im.Tag);
            ViewBag.Username = Username;
            return View(images);

        }

        [HttpGet]
        public ActionResult ListByUser()
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            // TODO Return form for selecting a user from a drop-down list
            ListByUserView userView = new ListByUserView();
            userView.Users = new SelectList(db.Users, "Id", "Username", 1);
            // TODO
            return View(userView);

        }

        [HttpGet]
        public ActionResult DoListByUser(ListByUserView userView)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            // TODO list all images uploaded by the user in userView (see List By Tag)
            User user = db.Users.Find(userView.Id);
            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "ListByUser" });
            }

            ViewBag.Username = Username;
            /*
             * Eager loading of related entities
             */
            var images = db.Entry(user).Collection(t => t.Images).Query().Include(im => im.Tag).ToList();
            // TODO
            return View("ListAll", user.Images);

        }

        [HttpGet]
        public ActionResult ListByTag()
        {
            CheckAda();
            if (GetLoggedInUser() == null)
            {
                return ForceLogin();
            }

            ListByTagView tagView = new ListByTagView();
            tagView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View(tagView);

        }

        [HttpGet]
        public ActionResult DoListByTag(ListByTagView tagView)
        {
            CheckAda();
            String Username = GetLoggedInUser();
            if (Username == null)
            {
                return ForceLogin();
            }

            Tag tag = db.Tags.Find(tagView.Id);
            if (tag == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "ListByTag" });
            }

            ViewBag.Username = Username;
            /*
             * Eager loading of related entities
             */
            var images = db.Entry(tag).Collection(t => t.Images).Query().Include(im => im.User).ToList();
            return View("ListAll", tag.Images);
        }
    }
}

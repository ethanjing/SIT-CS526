using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using ImageSharingWithSecurity.DAL;
using ImageSharingWithSecurity.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ImageSharingWithSecurity.Controllers
{
    [Authorize]
    public class ImagesController : BaseController
    {
        private readonly IHostingEnvironment hostingEnvironment;

        private readonly ILogger<ImagesController> logger;

        // Dependency injection
        public ImagesController(UserManager<ApplicationUser> userManager,
                                ApplicationDbContext db,
                                IHostingEnvironment environment,
                                ILogger<ImagesController> logger)
            : base(userManager, db)
        {
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
            //String Username = GetLoggedInUser();
            //if (Username == null)
            //{
            //    return ForceLogin();
            //}

            ViewBag.Message = "";
            ImageView imageView = new ImageView();
            imageView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View(imageView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(ImageView imageView)
        {
            CheckAda();

            //String Username = GetLoggedInUser();
            //if (Username == null)
            //{
            //    return ForceLogin();
            //}

            await TryUpdateModelAsync(imageView);

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please correct the errors in the form!";
                ViewBag.Tags = new SelectList(db.Tags, "Id", "Name", 1);
                return View();
            }

            ApplicationUser user = await GetLoggedInUser();
            //  User user = db.Users.SingleOrDefault(u => u.Username.Equals(Username));
            //if (user == null)
            //{
            //    ViewBag.Message = "No such Username registered!";
            //    return View();
            //}

            if (imageView.ImageFile == null || imageView.ImageFile.Length <= 0)
            {
                ViewBag.Message = "No image file specified!";
                imageView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
                return View(imageView);
            }

            // TODO save image metadata in the database 
            Image image = new Image();
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
            await imageView.ImageFile.CopyToAsync(new FileStream(imageDataFile(image.Id), FileMode.Create));
            // end TODO

            return RedirectToAction("Details", new { Id = image.Id });
        }

        [HttpGet]
        public ActionResult Query()
        {
            CheckAda();
            //if (GetLoggedInUser() == null)
            //{
            //    return ForceLogin();
            //}

            ViewBag.Message = "";
            return View();
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            CheckAda();
            //if (GetLoggedInUser() == null)
            //{
            //    return ForceLogin();
            //}

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
             * Eager loading of related entities
             */
            var imageEntry = db.Entry(image);
            imageEntry.Reference(i => i.Tag).Load();
            imageEntry.Reference(i => i.User).Load();
            imageView.TagName = image.Tag.Name;
            imageView.Username = image.User.UserName;
            return View(imageView);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int Id)
        {
            CheckAda();
            ApplicationUser user = await GetLoggedInUser();
            //if (user == null)
            //{
            //    return ForceLogin();
            //}

            Image image = db.Images.Find(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "EditNotFound" });
            }

            db.Entry(image).Reference(im => im.User).Load();  // Eager load of user
            if (!image.User.UserName.Equals(user.UserName))
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoEdit(int Id, ImageView imageView)
        {
            CheckAda();
            ApplicationUser user = await GetLoggedInUser();
            //if (user == null)
            //{
            //    return ForceLogin();
            //}

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
            if (!image.User.UserName.Equals(user.UserName))
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
        public async Task<ActionResult> Delete(int Id)
        {
            CheckAda();
            ApplicationUser user = await GetLoggedInUser();
            //if (user == null)
            //{
            //    return ForceLogin();
            //}

            Image image = db.Images.Find(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "Delete" });
            }

            db.Entry(image).Reference(im => im.User).Load();  // Explicit load of user
            if (!image.User.UserName.Equals(user.UserName))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "DeleteNotAuth" });
            }

            ImageView imageView = new ImageView();
            imageView.Id = image.Id;
            imageView.Caption = image.Caption;
            imageView.Description = image.Description;
            imageView.DateTaken = image.DateTaken;
            /*
             * Eager loading of related entities
             */
            db.Entry(image).Reference(i => i.Tag).Load();
            imageView.TagName = image.Tag.Name;
            imageView.Username = image.User.UserName;
            return View(imageView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoDelete(int Id)
        {
            CheckAda();
            ApplicationUser user = await GetLoggedInUser();
            //if (user == null)
            //{
            //    return ForceLogin();
            //}

            Image image = db.Images.Find(Id);
            if (image == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "DeleteNotFound" });
            }

            db.Entry(image).Reference(im => im.User).Load();  // Explicit load of user
            if (!image.User.UserName.Equals(user.UserName))
            {
                return RedirectToAction("Error", "Home", new { ErrId = "DeleteNotAuth" });
            }

            //db.Entry(imageEntity).State = EntityState.Deleted;
            db.Images.Remove(image);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<ActionResult> ListAll()
        {
            CheckAda();
            ApplicationUser user = await GetLoggedInUser();
            //if (Username == null)
            //{
            //    return ForceLogin();
            //}

            IList<Image> images = ApprovedImages().Include(im => im.User).Include(im => im.Tag).ToList();
            ViewBag.Username = user.UserName;
            return View(images);
        }

        [HttpGet]
        public ActionResult ListByUser()
        {
            CheckAda();
            //if (GetLoggedInUser() == null)
            //{
            //    return ForceLogin();
            //}

            // TODO Return form for selecting a user from a drop-down list
            List<SelectListItem> users = new List<SelectListItem>();
            foreach (var u in db.Users)
            {
                SelectListItem item = new SelectListItem { Text = u.UserName, Value = u.Id, Selected = true };
                users.Add(item);
            }

            ViewBag.message = "";
            ListByUserModel model = new ListByUserModel { Users = users };
            return View(model);
            // End TODO
        }

        [HttpGet]
        public async Task<ActionResult> DoListByUser(ListByUserModel userView)
        {
            CheckAda();
            // TODO list all images uploaded by the user in userView (see List By Tag)
            ApplicationUser currentUser = await GetLoggedInUser();
            var user = db.Users.Find(userView.Id);
            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "ListByUser" });
            }

            ViewBag.Username = currentUser.UserName;
            /*
             * Eager loading of related entities
             */
            var images = db.Entry(user).Collection(t => t.Images).Query().Include(im => im.Tag).ToList();
            return View("ListAll", user.Images);
            // End TODO


        }

        [HttpGet]
        public ActionResult ListByTag()
        {
            CheckAda();
            //if (GetLoggedInUser() == null)
            //{
            //    return ForceLogin();
            //}

            ListByTagModel tagView = new ListByTagModel();
            tagView.Tags = new SelectList(db.Tags, "Id", "Name", 1);
            return View(tagView);
        }

        [HttpGet]
        public async Task<ActionResult> DoListByTag(ListByTagModel tagView)
        {
            CheckAda();
            ApplicationUser user = await GetLoggedInUser();
            //String Username = GetLoggedInUser();
            //if (Username == null)
            //{
            //    return ForceLogin();
            //}

            Tag tag = db.Tags.Find(tagView.Id);
            if (tag == null)
            {
                return RedirectToAction("Error", "Home", new { ErrId = "ListByTag" });
            }

            ViewBag.Username = user.UserName;
            /*
             * Eager loading of related entities
             */
            var images = db.Entry(tag).Collection(t => t.Images).Query().Where(im => im.Approved).Include(im => im.User).ToList();
            return View("ListAll", tag.Images);
        }


        [HttpGet]
        [Authorize(Roles = "Approver")]
        public IActionResult Approve()
        {
            CheckAda();

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var im in db.Images)
            {
                SelectListItem item = new SelectListItem { Text =im.Caption, Value = im.Id.ToString(), Selected = im.Approved };
                items.Add(item);
            }

            ViewBag.message = "";
            ApproveModel model = new ApproveModel { Images = items };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Approver")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(ApproveModel model)
        {
            CheckAda();

            foreach (var item in model.Images.ToList())
            {
                Image image = db.Images.Find(int.Parse(item.Value));

                if (item.Selected)
                {
                    image.Approved = true;
                    model.Images.Remove(item);
                }
                else
                {
                    item.Text = image.Caption;
                }
            }

            await db.SaveChangesAsync();

            ViewBag.message ="Images approved!";

            return View(model);
        }
    }

}

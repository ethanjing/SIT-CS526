using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

using ImageSharingWithUpload.Models;

namespace ImageSharingWithUpload.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public ImagesController(IHostingEnvironment environment)
        {
            hostingEnvironment = environment;
        }

        protected void mkDirectories()
        {
            var dataDir = Path.Combine(hostingEnvironment.WebRootPath,
               "data", "images");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            var infoDir = Path.Combine(hostingEnvironment.WebRootPath,
               "data", "info");
            if (!Directory.Exists(infoDir))
            {
                Directory.CreateDirectory(infoDir);
            }
        }

        protected string imageDataFile(string id)
        {
            return Path.Combine(
               hostingEnvironment.WebRootPath,
               "data", "images", id + ".jpg");
        }

        protected string imageInfoFile(string id)
        {
            return Path.Combine(
               hostingEnvironment.WebRootPath,
               "data", "info", id + ".js");
        }

        protected void CheckAda()
        {
            var cookie = Request.Cookies["ADA"];
            if (cookie != null && "true".Equals(cookie))
            {
                ViewBag.isADA = true;
            }
            else
            {
                ViewBag.isADA = false;
            }
        }


        [HttpGet]
        public IActionResult Upload()
        {
            CheckAda();
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public IActionResult Upload(Image image,
                                    IFormFile imageFile)
        {
            CheckAda();

            if (ModelState.IsValid)
            {
                var userid = Request.Cookies["userid"];
                if (userid == null)
                {
                    return RedirectToAction("Register", "Account");
                }

                image.Userid = userid;

                /*
                 * Save image information on the server file system.
                 */
                var jsonData = JsonConvert.SerializeObject(image, Formatting.Indented);

                if (imageFile != null && imageFile.Length > 0)
                {
                    mkDirectories();
                    var stream = new FileStream(imageDataFile(image.Id), FileMode.Create);
                    imageFile.CopyToAsync(stream);
                    System.IO.File.WriteAllText(imageInfoFile(image.Id), jsonData);
                    return View("Details", image);
                }
                else
                {
                    ViewBag.Message = "No image file specified!";
                    return View(image);
                }

            }
            else
            {
                ViewBag.Message = "Please correct the errors in the form!";
                return View(image);
            }
        }

        [HttpGet]
        public IActionResult Query()
        {
            CheckAda();
            ViewBag.Message = "";
            return View();
        }

        [HttpGet]
        public IActionResult Details(Image image)
        {
            CheckAda();

            var userid = Request.Cookies["userid"];
            if (userid == null)
            {
                return RedirectToAction("Register", "Account");
            }

            String fileName = imageInfoFile(image.Id);
            if (System.IO.File.Exists(fileName))
            {
                String jsonData = System.IO.File.ReadAllText(fileName);
                Image imageInfo = JsonConvert.DeserializeObject<Image>(jsonData);

                return View(imageInfo);
            }
            else
            {
                ViewBag.Message = "Image with identifer " + image.Id + " not found";
                ViewBag.Id = image.Id;

                return View("Query");
            }

        }

    }
}
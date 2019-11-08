using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ImageSharingWithSecurity.Models;
using Microsoft.AspNetCore.Identity;
using ImageSharingWithSecurity.DAL;
using System.Threading.Tasks;

namespace ImageSharingWithSecurity.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext db) : base(userManager, db)
        {
        }


        public async Task<IActionResult> Index(String UserName = "Stranger")
        {
            CheckAda();
            ViewBag.Title = "Welcome!";
            ApplicationUser user = await GetLoggedInUser();
            if (user == null)
            {
                ViewBag.UserName = UserName;
            }
            else
            {
                ViewBag.UserName = user.UserName;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string ErrId)
        {
            CheckAda();
            return View(new ErrorViewModel { ErrId = ErrId, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

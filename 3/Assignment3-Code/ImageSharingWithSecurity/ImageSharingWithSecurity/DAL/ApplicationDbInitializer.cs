using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using ImageSharingWithSecurity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ImageSharingWithSecurity.DAL
{
    public static class ApplicationDbInitializer
    {
        public static async void Seed(IApplicationBuilder app)
        {
            ILogger logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();

            ApplicationDbContext db = null;
            // TODO get a database context from the application builder service
            // and ensure that the database has been migrated (tables created).
            // Also get a logger for debugging.

            db = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();

            // db.Database.EnsureCreated();
            db.Database.Migrate();

            db.RemoveRange(db.Images);
            db.RemoveRange(db.Tags);
            db.RemoveRange(db.Users);
            db.SaveChanges();

            logger.LogDebug("Adding role: User");
            var idResult = await CreateRole(app.ApplicationServices, "User");
            if (!idResult.Succeeded)
            {
                logger.LogDebug("Failed to create User role!");
            }

            logger.LogDebug("Adding role: Admin");
            idResult = await CreateRole(app.ApplicationServices, "Admin");
            if (!idResult.Succeeded)
            {
                logger.LogDebug("Failed to create Admin role!");
            }

            logger.LogDebug("Adding role: Approver");
            idResult = await CreateRole(app.ApplicationServices, "Approver");
            if (!idResult.Succeeded)
            {
                logger.LogDebug("Failed to create Approver role!");
            }
            // TODO add other roles

            logger.LogDebug("Adding user: jfk");
            idResult = await CreateAccount(app.ApplicationServices, "jfk@example.org", "jfk123", "User");
            if (!idResult.Succeeded)
            {
                logger.LogDebug("Failed to create jfk user!");
            }

            logger.LogDebug("Adding user: nixon");
            idResult = await CreateAccount(app.ApplicationServices, "nixon@example.org", "nixon123", "User");
            if (!idResult.Succeeded)
            {
                logger.LogDebug("Failed to create nixon user!");
            }

            logger.LogDebug("Adding admin: admin");
            idResult = await CreateAccount(app.ApplicationServices, "admin@example.org", "admin123", "Admin");
            if (!idResult.Succeeded)
            {
                logger.LogDebug("Failed to create Admin admin!");
            }

            logger.LogDebug("Adding approver: approver");
            idResult = await CreateAccount(app.ApplicationServices, "approver@example.org", "approver123", "Approver");
            if (!idResult.Succeeded)
            {
                logger.LogDebug("Failed to create Approver approver!");
            }
            // TODO add other users and assign more roles

            Tag portrait = new Tag { Name = "portrait" };
            db.Tags.Add(portrait);
            Tag architecture = new Tag { Name = "architecture" };
            db.Tags.Add(architecture); 
            Tag landscape = new Tag { Name = "landscape" };
            db.Tags.Add(landscape);

            // TODO add other tags

            db.SaveChanges();

        }

        public static async Task<IdentityResult> CreateRole(IServiceProvider provider,
                                                            string role)
        {
            RoleManager<IdentityRole> roleManager = provider
                .GetRequiredService
                       <RoleManager<IdentityRole>>();
            var idResult = IdentityResult.Success;
            if (await roleManager.FindByNameAsync(role) == null)
            {
                idResult = await roleManager.CreateAsync(new IdentityRole(role));
            }
            return idResult;
        }

        public static async Task<IdentityResult> CreateAccount(IServiceProvider provider,
                                                               string email, 
                                                               string password,
                                                               string role)
        {
            UserManager<ApplicationUser> userManager = provider
                .GetRequiredService
                       <UserManager<ApplicationUser>>();
            var idResult = IdentityResult.Success;

            if (await userManager.FindByNameAsync(email) == null)
            {
                ApplicationUser user = new ApplicationUser { UserName = email, Email = email };
                idResult = await userManager.CreateAsync(user, password);

                if (idResult.Succeeded)
                {
                    idResult = await userManager.AddToRoleAsync(user, role);
                }
            }

            return idResult;
        }

    }
}
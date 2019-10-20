using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using ImageSharingWithModel.Models;

namespace ImageSharingWithModel.DAL
{
    public static class ApplicationDbInitializer 
    {
        public static void Seed(IApplicationBuilder app)
        {
            ApplicationDbContext db = null;

            // TODO get a database context from the application builder service
            // and ensure that the database has been migrated (tables created).

            db = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();

            // db.Database.EnsureCreated();
            db.Database.Migrate();

            db.RemoveRange(db.Images);
            db.RemoveRange(db.Tags);
            db.RemoveRange(db.Users);
            db.SaveChanges();

            User jfk = new User { Username = "jfk", ADA = false };
            db.Users.Add(jfk);
            User nixon = new User { Username = "nixon", ADA = false };
            db.Users.Add(nixon);

            Tag portrait = new Tag { Name = "portrait" };
            db.Tags.Add(portrait);
            Tag architecture = new Tag { Name = "architecture" };
            db.Tags.Add(architecture);
            Tag landscape = new Tag { Name = "landscape" };
            db.Tags.Add(landscape);

            db.SaveChanges();

        }
    }
}
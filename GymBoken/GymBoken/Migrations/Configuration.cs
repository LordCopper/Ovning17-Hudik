namespace GymBoken.Migrations
{
    using GymBoken.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GymBoken.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GymBoken.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            string[] roleNames = new[]{ "Admin", "Member" };

            foreach (string roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    IdentityRole role = new IdentityRole { Name = roleName };
                    IdentityResult result = roleManager.Create(role);

                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            string[] emails = new[] { "john@leixcon.se", "admin@admin.ad", "editor@lexicon.se", "bob@lexicon.se", "admin@Gymbokning.se" };

            foreach (string email in emails)
            {
                if (!context.Users.Any(e => e.UserName == email))
                {
                    ApplicationUser user = new ApplicationUser { UserName = email, Email = email, };
                    var result = userManager.Create(user, "foobar");
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }

            ApplicationUser adminUser = userManager.FindByName("admin@admin.ad");
            userManager.AddToRole(adminUser.Id, "Admin");

            adminUser = userManager.FindByName("admin@Gymbokning.se");
            userManager.AddToRole(adminUser.Id, "Admin");

            foreach (ApplicationUser user in userManager.Users.ToList().Where(u => (u.Email != "admin@admin.ad" && u.Email != "admin@Gymbokning.se")))
            {
                userManager.AddToRole(user.Id, "Member");
            }
        }
    }
}

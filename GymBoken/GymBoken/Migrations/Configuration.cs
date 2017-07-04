using GymBoken.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GymBoken.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GymBoken.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GymBoken.Models.ApplicationDbContext context)
        {

            

            

           RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
           RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);
            
            string[] roleNames = new[] {"Admin", "Member"};

            foreach (var roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    IdentityRole role = new IdentityRole{ Name = roleName};
                    IdentityResult result = roleManager.Create(role);
                    
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }

                    
                }
            }
            

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new ApplicationUserManager(userStore);
            
            string[] emails = new[] {"anders@gym.se", "admin@admin.se", "borje@gym.se", "rumpnisse@gym.se"};

            foreach (var email in emails)
            {
                if (!context.Users.Any(u => u.UserName == email))
                {
                    ApplicationUser user = new ApplicationUser{UserName = email, Email = email};
                    var result = userManager.Create(user, "password");
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }

                }
               
            }
            
            ApplicationUser adminUser = userManager.FindByName("admin@admin.se");
            userManager.AddToRole(adminUser.Id, "Admin");



            foreach (ApplicationUser user in userManager.Users.Where(u => u.UserName != "admin@admin.se").ToList() )
            {
                userManager.AddToRole(user.Id, "Member");
            }

            

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
        }
    }
}

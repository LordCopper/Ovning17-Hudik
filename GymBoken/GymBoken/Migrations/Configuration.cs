using System.Collections.Generic;
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
            
            List<ApplicationUser> userslist = new List<ApplicationUser>();

            ApplicationUser user1 = new ApplicationUser{FirstName = "Bob", LastName = "Korv", Email = "Bob@korv.se", UserName = "Bob@korv.se", TimeOfRegistration = new DateTime(2016, 5, 1, 8, 30, 52) };
            userslist.Add(user1);
            ApplicationUser user2 = new ApplicationUser { FirstName = "Alfred", LastName = "Ballfred", Email = "Admin@Gymbokning.se", UserName = "Admin@Gymbokning.se", TimeOfRegistration = new DateTime(2014, 5, 5, 10, 34, 32) };
            userslist.Add(user2);
            foreach (var users in userslist)
            {
                if (!context.Users.Any(u => u.UserName == users.Email))
                {
                    ApplicationUser user = new ApplicationUser{UserName = users.UserName, Email = users.Email, FirstName = users.FirstName, LastName = users.LastName, TimeOfRegistration = users.TimeOfRegistration};
                    var result = userManager.Create(user, "password");
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }

                }
               
            }
            
            

            ApplicationUser adminUser = userManager.FindByName("admin@Gymbokning.se");
            userManager.AddToRole(adminUser.Id, "Admin");


            foreach (ApplicationUser user in userManager.Users.ToList().Where(u => u.Email != "admin@Gymbokning.se").ToList() )
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

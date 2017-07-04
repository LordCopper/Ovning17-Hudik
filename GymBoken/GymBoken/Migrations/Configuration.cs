namespace GymBoken.Migrations
{
    using GymBoken.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

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

            RoleStore<IdentityRole> roleStole = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStole);

            string[] roleNames = new[] { "Admin", "Member" };
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

            string[] emails = new[] { "johan@lexicon.se", "admin@admin.ad", "editor@lexicon.se", "bob@lexicon.se", "admin@Gymbokning.se" };
            string[] firstName = new[] { "Johan", "Abmin", "Bob", "Admin" };
            string[] lastName = new[] { "Hellman","Abmin","Wan", "obbsson", "GymBoken"};
            int i = 0;

            foreach (string email in emails)
            {
                if (!context.Users.Any(u => u.UserName == email))
                {
                    ApplicationUser user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName[i], LastName = lastName[i], TimeOfRegistration = DateTime.Now };
                    var result = userManager.Create(user, "foobar");
                    if (!result.Succeeded)
                    {

                        throw new Exception(string.Join("\n", result.Errors));

                    }
                }



            }
            ApplicationUser adminUser = userManager.FindByName("admin@admin.ad");
            userManager.AddToRole(adminUser.Id, "Admin");

            adminUser = adminUser = userManager.FindByName("admin@Gymbokning.se");
            userManager.AddToRole(adminUser.Id, "Admin");

            foreach (ApplicationUser user in userManager.Users.ToList().Where(u => u.Email != "admin@admin.ad" && u.Email != "admin@Gymbokning.se"))
            {
                userManager.AddToRole(user.Id, "Member");
            }

            context.SaveChanges();

            GymClass[] gym = new GymClass[] {
                new GymClass
                {
                Name = "Kicking",
                Description ="spraka loss",
                Duration = new TimeSpan(0,30,0),
                StartTime = new DateTime(1999,02,06),
                AttendingMembers = new List<ApplicationUser>()
   

                },

                 new GymClass
                {
                Name = "Running",
                Description =" Dom Tänker fånga dig",
                Duration = new TimeSpan(0,30,0),
                StartTime = new DateTime(2019,02,06),
                AttendingMembers = new List<ApplicationUser>()
            }

            };
            gym[0].AttendingMembers.Add(adminUser);
            gym[1].AttendingMembers.Add(adminUser);
            foreach (GymClass g in gym)
                {
                context.GymClass.Add(g);
                }
        }
        
    }
}

namespace GymBoken.Migrations
{
    using System;
    using GymBoken.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            string[] roleNames = new[] { "Admin", "Member" };
            foreach (string roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    IdentityRole identityRole = new IdentityRole { Name = roleName };
                    IdentityResult result = roleManager.Create(identityRole);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);
            string[] emails = new[] { "basse@nybom.se", "admin@admin.ad", "editor@edit.ed", "bob@basse.se","admin@Gymbokning.se" };
            string[] firstName = new[] { "John", "Abmin", "Editor", "Bob", "Admin" };
            string[] lastName = new[] { "Hellman", "Abmin", "Editor", "Hund", "Basse" };
            int i = 0;

            foreach(string email in emails)
            {
                if(!context.Users.Any(u=>u.UserName == email))
                {
                    ApplicationUser user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName[i], LastName = lastName[i], TimeOfRegistration = DateTime.Now};
                    var result = userManager.Create(user, "foobar");
                    if(!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
                i++;
            }
            ApplicationUser adminUser = userManager.FindByName("admin@admin.ad");
            userManager.AddToRole(adminUser.Id, "Admin");

            adminUser = userManager.FindByName("admin@Gymbokning.se");
            userManager.AddToRole(adminUser.Id, "Admin");

            foreach (ApplicationUser user in userManager.Users.ToList().Where(u => u.Email != "admin@admin.ad" && u.Email != "admin@Gymbokning.se")) 
            {
                userManager.AddToRole(user.Id,"Member");
            }
        }
    }
}

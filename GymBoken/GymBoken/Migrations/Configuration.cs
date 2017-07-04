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

			var roleStore = new RoleStore<IdentityRole>(context);
			var roleManager = new RoleManager<IdentityRole>(roleStore);

			string[] roleNames = new[] { "Admin", "Member" };
			foreach (string roleName in roleNames)
			{
				if (!context.Roles.Any(r => r.Name == roleName))
				{
					var role = new IdentityRole { Name = roleName };
					var result = roleManager.Create(role);
					if (!result.Succeeded)
					{
						throw new Exception(string.Join("\n", result.Errors));
					}
				}
			}

			var userStore = new UserStore<ApplicationUser>(context);
			var userManager = new UserManager<ApplicationUser>(userStore);

			string[] emails = new[] { "god@heaven.org", "nisse@hult.se", "a@b.com", "epa@bepa.net", "admin@gymbokning.se" };
			foreach (string email in emails)
			{
				if (!context.Users.Any(u => u.Email == email))
				{
					var user = new ApplicationUser { UserName = email, Email = email };
					var result = userManager.Create(user, "foobar");
					if (!result.Succeeded)
					{
						throw new Exception(string.Join("\n", result.Errors));
					}
				}
			}

			var adminUser = userManager.FindByEmail("god@heaven.org");
			userManager.AddToRole(adminUser.Id, "Admin");

			adminUser = userManager.FindByEmail("admin@gymbokning.se");
			userManager.AddToRole(adminUser.Id, "Admin");

			foreach (var user in userManager.Users.ToList().Where(u => (u.Email != "god@heaven.org" && u.Email != "admin@gymbokning.se")))
			{
				userManager.AddToRole(user.Id, "Member");
			}
		}
	}
}

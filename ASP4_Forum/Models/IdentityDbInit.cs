using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ASP4_Forum.Models
{
    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }
        public void PerformInitialSetup(ApplicationDbContext context)
        {
            City city1 = new City { CityName = "Одесса" };
            City city2 = new City { CityName = "Киев" };
            City city3 = new City { CityName = "Измаил" };
            City city4 = new City { CityName = "Николаев" };
            City city5 = new City { CityName = "Днепр" };
            City city6 = new City { CityName = "Львов" };
            City city7 = new City { CityName = "Минск" };
            City city8 = new City { CityName = "Гомель" };
            City city9 = new City { CityName = "Лондон" };
            City city10 = new City { CityName = "Манчестер" };
            City city25 = new City { CityName = "Берлин" };
            City city26 = new City { CityName = "Лейпцих" };
            City city27 = new City { CityName = "Дюссельдорф" };
            City city28 = new City { CityName = "Рим" };
            City city29 = new City { CityName = "Неаполь" };
            Country ctr1 = new Country { Name = "Україна", Cities = new List<City> { city1, city2, city3, city4, city5, city6 } };
            Country ctr2 = new Country { Name = "Беларусі", Cities = new List<City> { city7, city8 } };
            Country ctr3 = new Country { Name = "Great Britain", Cities = new List<City> { city9, city10 } };
            Country ctr11 = new Country { Name = "Deutschland", Cities = new List<City> { city25, city26, city27 } };
            Country ctr12 = new Country { Name = "Italia", Cities = new List<City> { city28, city29 } };
            context.Cities.AddRange(new List<City> { city1, city2, city3, city4, city5, city6, city7, city8, city9, city10, city25, city26, city27, city28, city29 });
            context.Countries.AddRange(new List<Country> { ctr1, ctr2, ctr3, ctr11, ctr12 });

            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            ApplicationRoleManager roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));
            string roleName = "Administrators";
            string userName = "admin123";
            string pass = "admin123";
            string email = "admin123@mail.com";
            string avatarPath = "Avatars/0.jpg";

            if (!roleManager.RoleExists(roleName))
                roleManager.Create(new ApplicationRole(roleName));

            ApplicationUser user = userManager.FindByName(userName);
            if (user == null)
            {
                var result = userManager.Create(new ApplicationUser { UserName = userName, Email = email, City = city1, Country = ctr1, AvatarURL = avatarPath }, pass);
                user = userManager.FindByName(userName);
            }

            if (!userManager.IsInRole(user.Id, roleName))
                userManager.AddToRole(user.Id, roleName);

            roleName = "Moderators";
            userName = "moder123";
            pass = "moder123";
            email = "moder123@mail.com";

            if (!roleManager.RoleExists(roleName))
                roleManager.Create(new ApplicationRole(roleName));
            user = null;
            user = userManager.FindByName(userName);

            if (user == null)
            {
                userManager.Create(new ApplicationUser { UserName = userName, Email = email, City = city2, Country = ctr1, AvatarURL = avatarPath }, pass);
                user = userManager.FindByName(userName);
            }

            if (!userManager.IsInRole(user.Id, roleName))
                userManager.AddToRole(user.Id, roleName);

            roleName = "Users";
            userName = "user123";
            pass = "user123";
            email = "user123@mail.com";

            if (!roleManager.RoleExists(roleName))
                roleManager.Create(new ApplicationRole(roleName));
            user = null;
            user = userManager.FindByName(userName);

            if (user == null)
            {
                userManager.Create(new ApplicationUser { UserName = userName, Email = email, City = city26, Country = ctr11, AvatarURL = avatarPath }, pass);
                user = userManager.FindByName(userName);
            }

            if (!userManager.IsInRole(user.Id, roleName))
                userManager.AddToRole(user.Id, roleName);


        }
    }

}
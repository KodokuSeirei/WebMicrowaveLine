using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMicrowaveLine.Models;

namespace WebMicrowaveLine
{
    public  class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Инициализация ролей и учетки админа + юзера, если их не существует
            string adminName = "Admin";
            string userName = "Vasya";
            string password = "123456";
            

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminName) == null)
            {
                User admin = new User { UserName = adminName, Email = "admin@WebMicrowaveLine.com", Birthday = "00.00.0000", Adress = "Улица", FullName = "Петрович", Position=0 };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            if (await userManager.FindByNameAsync(userName) == null)
            {
                User vasya = new User { UserName = userName, Email = "vasya@WebMicrowaveLine.com", Birthday = "22.10.2019", Adress = "Сидней", FullName = "Терещенко Василий Данилович", Position=0 };
                IdentityResult result = await userManager.CreateAsync(vasya, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(vasya, "user");
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Store.Core.Entites.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManger)
        {
            if(!userManger.Users.Any()) {
                var User = new AppUser()
                {
                    DisplayName = "Aliaa Tarek",
                    Email = "aliaatarek.route@gmail.com",
                    UserName = "aliaatarek.route",
                    PhoneNumber = "01234567891",
                };
                await userManger.CreateAsync(User, "Pa$$w0rd");

            }

        }
    }
}

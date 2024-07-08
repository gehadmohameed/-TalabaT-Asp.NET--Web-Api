using Microsoft.AspNetCore.Identity;
using Store.Core.Entites.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services
{
    public interface ItokenServices
    {

        Task<string> CreateTakenAsync(AppUser User , UserManager<AppUser> userManager);
    }
}

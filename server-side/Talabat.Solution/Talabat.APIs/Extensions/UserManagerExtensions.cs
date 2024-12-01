using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Domain.Layer.Entities.Identity;

namespace Talabat.APIs.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindByEmailAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal currentUser)
        {
            var email = currentUser.FindFirstValue(ClaimTypes.Email);            
            return (await userManager.Users.Include(user => user.Address).SingleOrDefaultAsync(user => user.Email == email))!;
        }
    }
}

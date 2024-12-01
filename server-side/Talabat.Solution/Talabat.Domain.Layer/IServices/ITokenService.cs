using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities.Identity;

namespace Talabat.Domain.Layer.IServices
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}

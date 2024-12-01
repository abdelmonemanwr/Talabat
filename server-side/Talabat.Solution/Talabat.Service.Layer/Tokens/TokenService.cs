using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities.Identity;
using Talabat.Domain.Layer.IServices;

namespace Talabat.Service.Layer.Tokens
{
    public class TokenService : ITokenService
    {
        public IConfiguration Configuration { get; }
        
        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public async Task<string> GenerateToken(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            // Private Claims (user-defined claims)
            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach(var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));


            string? secretKey = Configuration["Jwt:SecretKey"];
            if (secretKey is null)
            {
                throw new ArgumentNullException("Secret Key is not set");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            
            double days = 0;
            double.TryParse(Configuration["Jwt:AccessTokenDurationInDays"], out days);

            var JwtSecurityToken = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                expires: DateTime.Now.AddDays(days),
                claims: authClaims,
                signingCredentials: signingCredentials // Signature
            );

            string token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);

            return token;
        }
    }
}

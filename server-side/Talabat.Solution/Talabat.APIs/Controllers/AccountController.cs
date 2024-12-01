using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.AuthDTOs;
using Talabat.APIs.DTOs.ErrorsDTOs;
using Talabat.Domain.Layer.Entities.Identity;
using Talabat.Domain.Layer.IServices;
using Talabat.APIs.Extensions;

namespace Talabat.APIs.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService , IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDTO)
        {
            ApplicationUser? appUser = await userManager.FindByEmailAsync(loginDTO.Email);
            if(appUser == null)
            {
                return Unauthorized(new ErrorResponseDTO { isSuccess = false, Message = "Invalid Email or Password" });
            }

            var result = await signInManager.CheckPasswordSignInAsync(appUser, loginDTO.Password, false);
            if(!result.Succeeded)
            {
                return Unauthorized(new ErrorResponseDTO { isSuccess = false, Message = "Invalid Email or Password" });
            }

            return Ok(new AuthResponseDTO
            {
                isSuccess = true,
                Message = "Login Successful",
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                Token = await tokenService.GenerateToken(appUser, userManager),
                RefreshToken = "RefreshToken",
                ExpiryDate = (loginDTO.RememberMe) ? DateTime.Now.AddDays(3) : DateTime.Now.AddDays(1)
            });
        }

        [HttpGet("check-existing-email")]
        public async Task<ActionResult<bool>> CheckExistingEmail(string email)
        {
            return Ok(await userManager.FindByEmailAsync(email) != null);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO registerDTO)
        {
            var appUser = mapper.Map<ApplicationUser>(registerDTO);
            bool emailExist = CheckExistingEmail(appUser.Email!).Result.Value;
            if (emailExist)
            {
                return BadRequest(new ErrorResponseDTO { isSuccess = false, Message = "Email already exists" });
            }
            var result = await userManager.CreateAsync(appUser, registerDTO.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"{{ {error.Description} }} \n");
                    Console.WriteLine();
                }
                return BadRequest(new ErrorResponseDTO { isSuccess = false, Message = "Invalid Email or Password" });
            }

            return Ok(new AuthResponseDTO
            {
                isSuccess = true,
                Email = appUser.Email,
                DisplayName = appUser.DisplayName,
                Message = $"{appUser.DisplayName} registered successfully",
                Token = await tokenService.GenerateToken(appUser, userManager),
            });
        }

        [HttpPost("logout"), Authorize]
        public async Task<ActionResult<AuthResponseDTO>> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new
            {
                isSuccess = true,
                Message = "Logout Successful"
            });
        }

        [HttpGet("get-loggedIn-user"), Authorize]
        public async Task<ActionResult<AuthResponseDTO>> GetLoggedInUser()
        {
            string? email = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser? user = await userManager.FindByEmailAsync(email!);
            return Ok(new AuthResponseDTO
            {
                isSuccess = true,
                DisplayName = user?.DisplayName,
                Email = user?.Email,
                Message = "User Found",
                Token = await tokenService.GenerateToken(user!, userManager)
            });
        }

        [HttpGet("get-user-address"), Authorize]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            // My Extension Method that include Address in the query
            ApplicationUser user = await userManager.FindByEmailAsync(User);
            return Ok(new { Address = mapper.Map<Address, AddressDTO>(user.Address) });
        }

        [HttpPut("update-user-address"), Authorize]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO newAddressDTO)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(User); // My Extension Method that include Address in the query
            user.Address = mapper.Map<AddressDTO, Address>(newAddressDTO);
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ErrorResponseDTO { isSuccess = false, Message = "Failed to update address" });
            }

            return Ok(new { isSuccess = true, Message = "Address updated successfully", Address = mapper.Map<Address, AddressDTO>(user.Address) });
        }

        //[HttpPost("refreshToken")]
        //public async Task<ActionResult<AuthResponseDTO>> RefreshToken(RefreshTokenDTO refreshTokenDTO)
        //{
        //    var principal = tokenService.GetPrincipalFromExpiredToken(refreshTokenDTO.Token);
        //    var email = principal.Identity.Name;
        //    var user = await userManager.FindByEmailAsync(email);
        //    if (user == null || user.RefreshToken != refreshTokenDTO.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        //    {
        //        return BadRequest(new ErrorResponseDTO { isSuccess = false, Message = "Invalid Token" });
        //    }
        //    return Ok(new AuthResponseDTO
        //    {
        //        isSuccess = true,
        //        Email = user.Email,
        //        DisplayName = user.DisplayName,
        //        Token = await tokenService.GenerateToken(user, userManager),
        //    });
        //}
    }
}

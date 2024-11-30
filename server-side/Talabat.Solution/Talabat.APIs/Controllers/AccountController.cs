using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs.AuthDTOs;
using Talabat.APIs.DTOs.ErrorsDTOs;
using Talabat.Domain.Layer.Entities.Identity;

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
        private readonly IMapper mapper;
        
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                Token = "Token",
                RefreshToken = "RefreshToken",
                ExpiryDate = (loginDTO.RememberMe) ? DateTime.Now.AddDays(3) : DateTime.Now.AddDays(1)
            });
        }


        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO registerDTO)
        {
            var appUser = mapper.Map<ApplicationUser>(registerDTO);

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
            });

        }

    }
}

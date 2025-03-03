using Karage.Application.Interfaces; 
using Karage.Domain.Common;
using Karage.Domain.Entities; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Karage.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtService jwtService;
        private readonly IQuickBooksService quickBooksService;
        public record LoginDTO(string Email, string Password) ;

        public AuthController(UserManager<ApplicationUser> userManager, IJwtService jwtService, IQuickBooksService quickBooksService)
        {
            this.userManager = userManager;
            this.jwtService = jwtService;
            this.quickBooksService = quickBooksService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO) 
        {
            ResponseVM response = new ResponseVM { Status =  200 , Message ="Success"};
            try
            {
                var user = await userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null || !await userManager.CheckPasswordAsync(user, loginDTO.Password))
                {
                    response.Status = 401;
                    response.Message = "Invalid email or password";
                }
                else
                { 
                    var token = jwtService.GenerateToken(user);
                    bool IsQBConnected = await quickBooksService.IsRefreshTokenActive();
                    response.Data = new { token, IsQBConnected };
                }
            }
            catch (Exception ex) 
            { 
                response.Status = 500;  
                response.Message = ex.Message;
            }

            return Ok(response);
        }


    }
}

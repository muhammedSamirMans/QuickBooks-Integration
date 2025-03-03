using Karage.Application.Interfaces; 
using Karage.Domain.Common;
using Karage.Domain.Entities; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
namespace Karage.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuickBooksController : ControllerBase
    {
        private readonly IQuickBooksService quickBooksService;
        public QuickBooksController(IQuickBooksService quickBooksService)
        {
            this.quickBooksService = quickBooksService;
            
        }
        [HttpGet("connect-quickbooks")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Connect()
        {
            string url = await quickBooksService.getAuthorizeUrl();
            return Ok(new {Status = 200, URL = url});
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            var state = Request.Query["state"];
            var code = Request.Query["code"].ToString();
            var realmId = Request.Query["realmId"].ToString();
            QBAuth token = new QBAuth();
            if (state.Count > 0 && !string.IsNullOrEmpty(code))
            {
               token =  await quickBooksService.GetAuthTokensAsync(code, realmId);
            }
             

            return Ok(new { AccessToken = token.AccessToken, RefreshToken = token.RefreshToken });
        }
         
        [HttpGet("GetCompanyInfo-quickbooks")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCompanyInfo()
        {
            ApiResponse response = await quickBooksService.GetCompanyInfoAsync();
            return Ok(new { Status = 200, Result = response });
        }
    }
}
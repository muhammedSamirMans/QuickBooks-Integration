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
            string url = await quickBooksService.GetAuthorizeUrl();
            return Ok(new {Status = 200, URL = url});
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            ResponseVM response = new ResponseVM { Status = 200, Message = "Success" };
            var state = Request.Query["state"];
            var code = Request.Query["code"].ToString();
            var realmId = Request.Query["realmId"].ToString();
            QBAuth token = new QBAuth();
            if (state.Count > 0 && !string.IsNullOrEmpty(code))
            {
                try
                {
                    token = await quickBooksService.GetAuthTokensAsync(code, realmId);
                }
                catch (Exception ex) 
                {
                    response.Message = ex.Message;
                    response.Status = 500;
                }
            }
            else
            {
                response.Message = "Sorry, code is empty";
                response.Status = 403;
            }
            return Ok(response);
        }
         
        [HttpGet("GetCompanyInfo-quickbooks")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCompanyInfo()
        {
            ResponseVM response = new ResponseVM { Status = 200, Message = "Success" };

            try
            {
                ApiResponse apiResponse = await quickBooksService.GetCompanyInfoAsync();
            }
            catch (Exception ex)
            { 
                response.Message = ex.Message;
                response.Status = 500;
            } 

            return Ok(response);
        }
    }
}
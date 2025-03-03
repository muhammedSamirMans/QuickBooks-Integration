using Karage.Domain.Common;
using Karage.Domain.Common.DTOs;
using Karage.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;

namespace Karage.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService invoiceService;
        public InvoicesController(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;   
        }
        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll() 
        {
            return Ok(new ResponseVM
            {
                Status = 200,
                Data = await invoiceService.readAll(),
                Message = "Success"
            });
        }
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody]InvoiceDTO invoiceDTO)
        {
            return Ok(await invoiceService.Create(invoiceDTO)); 
        }
    }
}

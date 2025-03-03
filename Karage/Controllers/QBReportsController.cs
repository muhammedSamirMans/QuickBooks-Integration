using Karage.Domain.Common.DTOs;
using Karage.Domain.Interfaces; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;

namespace Karage.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QBReportsController : ControllerBase
    {
        private readonly IQBReportService qBReportService;
        public QBReportsController(IQBReportService qBReportService)
        {
            this.qBReportService = qBReportService;
        }
        [HttpPost("Report")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody] QBReportDTO reportDTO)
        {
            return Ok(await qBReportService.GetReportAsync(reportDTO));
        }
    }
}

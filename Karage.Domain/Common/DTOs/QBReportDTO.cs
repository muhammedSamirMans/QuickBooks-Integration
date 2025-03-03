
using System.ComponentModel.DataAnnotations;

namespace Karage.Domain.Common.DTOs
{
    public class QBReportDTO
    {
        [Required]
        public string reportName { get; set; }

        public DateTime? startDate { get; set; } = null;
        public DateTime? endDate { get; set; } = null;
    }
}

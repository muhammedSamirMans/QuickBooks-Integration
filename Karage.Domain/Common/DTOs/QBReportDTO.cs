using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karage.Domain.Common.DTOs
{
    public class QBReportDTO
    {
        public string reportName { get; set; }
        public DateTime? startDate { get; set; } = null;
        public DateTime? endDate { get; set; } = null;
    }
}

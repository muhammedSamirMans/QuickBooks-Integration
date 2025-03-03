using Karage.Domain.Common;
using Karage.Domain.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karage.Domain.Interfaces
{
    public interface IQBReportService
    {
        Task<ResponseVM> GetReportAsync(QBReportDTO reportDTO);
    }
}

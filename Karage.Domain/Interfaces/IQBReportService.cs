using Karage.Domain.Common;
using Karage.Domain.Common.DTOs;

namespace Karage.Domain.Interfaces
{
    public interface IQBReportService
    {
        Task<ResponseVM> GetReportAsync(QBReportDTO reportDTO);
    }
}

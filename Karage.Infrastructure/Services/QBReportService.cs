using Intuit.Ipp.Data;
using Intuit.Ipp.ReportService;
using Intuit.Ipp.Core;
using Karage.Application.Interfaces;
using Karage.Domain.Common;
using Karage.Domain.Common.DTOs;
using Karage.Domain.Interfaces;
using System.Runtime.Serialization;

namespace Karage.Infrastructure.Services
{
    public class QBReportService: IQBReportService
    {
        private readonly IQuickBooksService quickBooksService;

        public QBReportService(IQuickBooksService quickBooksService)
        {
            this.quickBooksService = quickBooksService;
        }

        public async Task<ResponseVM> GetReportAsync(QBReportDTO reportDTO)
        {
            ResponseVM response = new ResponseVM { Status = 200, Message = "Success" };
            try
            { 
                var serviceContext = await quickBooksService.GetServiceContextAsync(); 
                var reportService = new ReportService(serviceContext);

                reportService.start_date = reportDTO.startDate?.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.AddMonths(-1).ToString("yyyy-MM-dd");
                reportService.end_date = reportDTO.endDate?.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
              
                // Fetch the requested report
                Report report = reportService.ExecuteReport(reportDTO.reportName);
                response.Data = report;
            }
            catch (SerializationException serEx)
            {
                Console.WriteLine($"❌ Serialization Error: {serEx.Message}");
                if (serEx.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner Exception: {serEx.InnerException.Message}");
                }
                throw;
            }
            catch (Exception ex)
            {
               response.Status = 500;   
               response.Message = ex.Message;
            }
            return response;
        }
    }
}

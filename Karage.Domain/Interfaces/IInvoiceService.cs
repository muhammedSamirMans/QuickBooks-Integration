using Karage.Domain.Common.DTOs;
using Karage.Domain.Common;
using Intuit.Ipp.Data;

namespace Karage.Domain.Interfaces
{
    public interface IInvoiceService
    {
        #region Invoices
        Task<List<InvoiceVM>> readAll();
        Task<InvoiceVM> readById(int invoiceId);
        Task<ResponseVM> Create(InvoiceDTO invoiceDTO);
        #endregion
    }
}

using Karage.Domain.Common;
using Karage.Domain.Common.DTOs;
using Karage.Domain.Interfaces;
using Intuit.Ipp.Data;
using Intuit.Ipp.QueryFilter;
using Karage.Domain.Entities;
using Karage.Application.Interfaces;
using Intuit.Ipp.DataService;

namespace Karage.Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    { 
        private readonly IQuickBooksService quickBooksService;
        public InvoiceService( IQuickBooksService quickBooksService)
        {
            this.quickBooksService = quickBooksService;
        }

        public async Task<ResponseVM> Create(InvoiceDTO invoiceDTO)
        {
            ResponseVM responseVM = new ResponseVM();
            try
            {
                var serviceContext = await quickBooksService.GetServiceContextAsync();
                var dataService = new DataService(serviceContext);
                 
                #region Create Inovice
                // 🔥 Create a new Invoice object
                var line = new Line
                {
                    DetailType = LineDetailTypeEnum.SalesItemLineDetail,
                    DetailTypeSpecified = true,
                    Description = "Create Invoice From Karage App.",
                    Amount = invoiceDTO.TotalAmount,
                    AmountSpecified = true
                };
                var lineDetail = new SalesItemLineDetail
                {
                    ItemRef = new ReferenceType { name = "", Value = invoiceDTO.ItemRef }
                };
                line.AnyIntuitObject = lineDetail;

                Line[] lines = { line };

                var newInvoice = new Invoice
                {
                    Line = lines,
                    CustomerRef = new ReferenceType { name = "", Value = invoiceDTO.CustomerRef },
                    TxnDate = DateTime.Now.Date
                };
                #endregion

                // 🔥 Send the invoice to QuickBooks
                Invoice createdInvoice = dataService.Add(newInvoice);

                var result = new InvoiceVM
                {
                    Id = createdInvoice.Id,
                    CustomerRef = createdInvoice.CustomerRef?.Value,
                    TotalAmount = createdInvoice.TotalAmt,
                    BalanceDue = createdInvoice.Balance,
                    InvoiceDate = createdInvoice.TxnDate
                };
                responseVM.Status = 200;
                responseVM.Data = result;
                responseVM.Message = "Success";
                return responseVM;
            }
            catch (Exception ex)
            {
                responseVM.Status = 500; 
                responseVM.Message = ex.Message;
                return responseVM; 
            } 
        }

        public async Task<List<InvoiceVM>> readAll()
        {
            var serviceContext = await quickBooksService.GetServiceContextAsync();
            // Use QueryService with manually set token
            var queryService = new QueryService<Invoice>(serviceContext);

            try
            {
                List<Invoice> invoices = queryService.ExecuteIdsQuery("SELECT * FROM Invoice").ToList();
                // Map to ViewModel
                return invoices.Select(invoice => new InvoiceVM
                {
                    Id = invoice.Id,
                    CustomerRef = invoice.CustomerRef?.Value,
                    TotalAmount = invoice.TotalAmt,
                    BalanceDue = invoice.Balance,
                    InvoiceDate = invoice.TxnDate
                }).ToList();

            }
            catch (Exception ex)
            {

                // Map to ViewModel
                return null;
            }

        }

        public Task<InvoiceVM> readById(int invoiceId)
        {
            throw new NotImplementedException();
        }

    }

}

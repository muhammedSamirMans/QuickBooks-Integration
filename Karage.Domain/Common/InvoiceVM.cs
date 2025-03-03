using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karage.Domain.Common
{
    public class InvoiceVM
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? BalanceDue { get; set; }
        public System.DateTime? InvoiceDate { get; set; }
    }
}

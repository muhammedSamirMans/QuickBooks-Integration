using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karage.Domain.Common.DTOs
{
    public class InvoiceDTO
    {
        public  string CustomerRef { get; set; }
        public  string ItemRef { get; set; }
        public decimal TotalAmount { get; set; }
    }
   
}

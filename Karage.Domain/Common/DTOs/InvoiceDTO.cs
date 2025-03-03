using System.ComponentModel.DataAnnotations; 

namespace Karage.Domain.Common.DTOs
{
    public class InvoiceDTO
    {
        [Required]
        public  string CustomerRef { get; set; }
        [Required]
        public  string ItemRef { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
    }
   
}

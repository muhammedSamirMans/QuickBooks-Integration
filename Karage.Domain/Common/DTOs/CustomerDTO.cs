

using System.ComponentModel.DataAnnotations;

namespace Karage.Domain.Common.DTOs
{
    public class CustomerDTO
    {
        [Required]
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }
}

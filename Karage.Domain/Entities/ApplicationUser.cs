
using Microsoft.AspNetCore.Identity;

namespace Karage.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}

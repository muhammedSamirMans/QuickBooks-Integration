using Karage.Domain.Entities;          

namespace Karage.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
    }
}

using Intuit.Ipp.Core;
using Intuit.Ipp.OAuth2PlatformClient;
using Karage.Domain.Common;
using Karage.Domain.Entities; 

namespace Karage.Application.Interfaces
{
    public interface IQuickBooksService
    {
        #region OAuth 2
        Task<string> GetAuthorizeUrl();
        Task<QBAuth> GetAuthTokensAsync(string code, string realmId);
        Task<TokenResponse> RefreshAccessTokenAsync(string refreshToken);
        Task<QBAuth> GetActiveTokenAsync();
        Task<bool> IsRefreshTokenActive();
        #endregion

        Task<ApiResponse> GetCompanyInfoAsync();

        Task<ServiceContext> GetServiceContextAsync(); 
    }
}

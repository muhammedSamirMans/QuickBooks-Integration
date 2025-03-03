#region Using
using Intuit.Ipp.Core; 
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.Security;
using Karage.Application.Interfaces;
using Karage.Domain.Common;
using Karage.Domain.Entities;
using Karage.Domain.Interfaces;
using Karage.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration; 
using System.Net.Http.Headers;
using System.Text.Json;
#endregion
namespace Karage.Infrastructure.Services
{
    public class QuickBooksService: IQuickBooksService
    {
        #region Variable Definisions
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly IGenericRepository<QBAuth> qbAuthRepository;
        private readonly IUnitOfWork unitOfWork;
        private OAuth2Client oAuth2Client;
        #endregion

        #region CTOR
        public QuickBooksService(HttpClient httpClient, IConfiguration configuration, IGenericRepository<QBAuth> qbAuthRepository, IUnitOfWork unitOfWork)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
            this.qbAuthRepository = qbAuthRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region OAuth 2 Functions
        public async Task<string> GetAuthorizeUrl()
        {
            var quickBooksSettings = configuration.GetSection("QuickBooks").Get<QuickBooksSettings>();
            oAuth2Client = new OAuth2Client(quickBooksSettings.ClientId, quickBooksSettings.ClientSecret, quickBooksSettings.RedirectUrl, quickBooksSettings.Environment);
            var scopes = new List<OidcScopes>();
            scopes.Add(OidcScopes.Accounting);
            var authorizeUrl = oAuth2Client.GetAuthorizationURL(scopes);
            return authorizeUrl;
        }

        public async Task<QBAuth> GetAuthTokensAsync(string code, string realmId)
        {
            var quickBooksSettings = configuration.GetSection("QuickBooks").Get<QuickBooksSettings>();
            oAuth2Client = new OAuth2Client(quickBooksSettings.ClientId, quickBooksSettings.ClientSecret, quickBooksSettings.RedirectUrl,
                quickBooksSettings.Environment);
            var tokenResponse = await oAuth2Client.GetBearerTokenAsync(code);
            quickBooksSettings.RealmId = realmId;
            var token = await qbAuthRepository.FirstOrDefaultAsync(t => t.RealmId == realmId && !t.IsExpired);
            if (token == null)
            {
                await qbAuthRepository.AddAsync(new QBAuth
                {
                    RealmId = realmId,
                    AccessToken = tokenResponse.AccessToken,
                    RefreshToken = tokenResponse.RefreshToken,
                    ExpiryDate = DateTime.UtcNow.AddSeconds(tokenResponse.AccessTokenExpiresIn),
                    RefreshTokenExpiryDate = DateTime.UtcNow.AddSeconds(tokenResponse.RefreshTokenExpiresIn)

                });
                await unitOfWork.SaveChangesAsync();
                token = await qbAuthRepository.FirstOrDefaultAsync(t => t.RealmId == realmId && !t.IsExpired);

            }
            return token;
        }

        // Check if token is expired and refresh it
        public async Task<QBAuth> GetActiveTokenAsync()
        {
            var token = await qbAuthRepository.FirstOrDefaultAsync(t => !t.IsExpired);

            if (token.ExpiryDate <= DateTime.UtcNow)
            {
                var newTokenResponse = await RefreshAccessTokenAsync(token.RefreshToken);
                token.AccessToken = newTokenResponse.AccessToken;
                token.RefreshToken = newTokenResponse.RefreshToken;
                token.ExpiryDate = DateTime.UtcNow.AddSeconds(newTokenResponse.AccessTokenExpiresIn);

                await qbAuthRepository.UpdateAsync(token);
                await unitOfWork.SaveChangesAsync();
            }
            return token;
        }

        public async Task<bool> IsRefreshTokenActive()
        {
            var token = await qbAuthRepository.FirstOrDefaultAsync(t => !t.IsExpired);
            if (token == null)
            {
                return false;
            }
            else
            {
                if (token.RefreshTokenExpiryDate <= DateTime.UtcNow)
                {
                    token.IsExpired = true;
                    await qbAuthRepository.UpdateAsync(token);
                    await unitOfWork.SaveChangesAsync();
                    return false;
                }
                else
                    return true;
            }

        }

        public async Task<TokenResponse> RefreshAccessTokenAsync(string refreshToken)
        {
            var quickBooksSettings = configuration.GetSection("QuickBooks").Get<QuickBooksSettings>();
            OAuth2Client auth2Client = new OAuth2Client(quickBooksSettings.ClientId, quickBooksSettings.ClientSecret, quickBooksSettings.RedirectUrl, quickBooksSettings.Environment);
            TokenResponse tokenResponse = await auth2Client.RefreshTokenAsync(refreshToken);
            return tokenResponse;
        }

        #endregion

        public async Task<ApiResponse> GetCompanyInfoAsync()
        {
            var token = await qbAuthRepository.FirstOrDefaultAsync(t => !t.IsExpired);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.GetAsync($"https://sandbox-quickbooks.api.intuit.com/v3/company/{token.RealmId}/companyinfo/{token.RealmId}");
            response.EnsureSuccessStatusCode();
            var jsonResponse =  await response.Content.ReadAsStringAsync();
            //QueryResponse queryResponse = JsonSerializer.Deserialize<QueryResponse>(jsonResponse);
            ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);//new ApiResponse { QueryResponse = queryResponse, Time = DateTime.UtcNow};
            return apiResponse;
        } 

        public async Task<ServiceContext> GetServiceContextAsync()
        {
            var quickBooksSettings = configuration.GetSection("QuickBooks").Get<QuickBooksSettings>();
            var token = await GetActiveTokenAsync();
            if (token == null) throw new UnauthorizedAccessException("No valid access token found.");

            //Ensure we're using the correct environment
            var authValidator = new OAuth2RequestValidator(token.AccessToken);
            var serviceContext = new ServiceContext(token.RealmId, IntuitServicesType.QBO, authValidator);
            serviceContext.IppConfiguration.MinorVersion.Qbo = "65";
            serviceContext.IppConfiguration.BaseUrl.Qbo = quickBooksSettings.BaseUrl;
            serviceContext.IppConfiguration.Message.Request.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
            serviceContext.IppConfiguration.Message.Response.SerializationFormat = Intuit.Ipp.Core.Configuration.SerializationFormat.Json;
            return serviceContext;
        }
    }
}

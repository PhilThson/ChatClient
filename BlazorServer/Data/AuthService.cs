using System.IdentityModel.Tokens.Jwt;
using BlazorServer.Helpers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorServer.Data
{
    public class AuthService
	{
        private readonly HttpClient _httpClient;
        private readonly ProtectedSessionStorage _sessionStore;

        public AuthService(IHttpClientFactory httpClientFactory,
            ProtectedSessionStorage protectedSessionStore)
        {
            _httpClient = httpClientFactory.CreateClient(ChatConstants.HttpAuthClient);
            _sessionStore = protectedSessionStore;
        }

        public async Task Authenticate(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync("authenticate", loginModel);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<AuthResponse>();

            await _sessionStore.SetAsync(ChatConstants.JwtName, content?.JwtToken);
            await _sessionStore.SetAsync(ChatConstants.RefreshTokenName, content?.RefreshToken);
            await _sessionStore.SetAsync(ChatConstants.UserEmail, loginModel.Email);
        }

        public async Task RenewTokenAsync()
        {
            var refreshTokenResult = await _sessionStore.GetAsync<string>("refreshToken");
            if (refreshTokenResult.Success)
            {
                string refreshToken = refreshTokenResult.Value;

                var tokenRenewalResponse = await _httpClient.PostAsJsonAsync(
                    "refreshToken", new { Token = refreshToken });

                if (tokenRenewalResponse.IsSuccessStatusCode)
                {
                    var tokenRenewalContent = await tokenRenewalResponse.Content.ReadFromJsonAsync<AuthResponse>();
                    if (tokenRenewalContent != null && !string.IsNullOrEmpty(tokenRenewalContent.JwtToken))
                    {
                        await _sessionStore.SetAsync(ChatConstants.JwtName, tokenRenewalContent.JwtToken);
                        await _sessionStore.SetAsync(ChatConstants.RefreshTokenName, tokenRenewalContent.RefreshToken);
                    }
                    else
                    {
                        // Handle the case where token renewal was not successful
                        // Redirect to login?
                    }
                }
                else
                {
                    // Handle the case where the token renewal request was not successful
                    // Redirect to login / errorMessage
                }
            }
        }

        public JwtSecurityToken ParseJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        public string GetClaimValue(string token, string claimType)
        {
            var parsedToken = ParseJwtToken(token);
            var claim = parsedToken.Claims.FirstOrDefault(c => c.Type == claimType);
            return claim?.Value;
        }
    }
}


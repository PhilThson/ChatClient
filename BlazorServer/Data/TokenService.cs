using System;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorServer.Data
{
	public class TokenService
	{
        private readonly HttpClient httpClient;
        private readonly ProtectedSessionStorage protectedSessionStore;

        public TokenService(HttpClient httpClient,
            ProtectedSessionStorage protectedSessionStore)
        {
            this.httpClient = httpClient;
            this.protectedSessionStore = protectedSessionStore;
        }

        public async Task RenewTokenAsync()
        {
            var refreshTokenResult = await protectedSessionStore.GetAsync<string>("refreshToken");
            if (refreshTokenResult.Success)
            {
                string refreshToken = refreshTokenResult.Value;

                var tokenRenewalResponse = await httpClient.PostAsJsonAsync(
                    "https://localhost:7178/refreshToken",
                    new { Token = refreshToken });

                if (tokenRenewalResponse.IsSuccessStatusCode)
                {
                    var tokenRenewalContent = await tokenRenewalResponse.Content.ReadFromJsonAsync<AuthResponse>();
                    if (tokenRenewalContent != null && !string.IsNullOrEmpty(tokenRenewalContent.JwtToken))
                    {
                        await protectedSessionStore.SetAsync("jwtToken", tokenRenewalContent.JwtToken);
                        await protectedSessionStore.SetAsync("refreshToken", tokenRenewalContent.RefreshToken);
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
    }
}


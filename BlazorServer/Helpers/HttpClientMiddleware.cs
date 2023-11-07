using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorServer.Helpers
{
    public class HttpClientMiddleware : DelegatingHandler
    {
        private readonly ProtectedSessionStorage _sessionStore;

        public HttpClientMiddleware(ProtectedSessionStorage sessionStore)
        {
            _sessionStore = sessionStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            //var result = await _sessionStore.GetAsync<string>(ChatConstants.JwtName);
            //var jwt = result.Success ? result.Value : "";

            //if (!string.IsNullOrEmpty(jwt))
            //    request.Headers.Add("Authorization", "Bearer " + jwt);
            _ = request;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}


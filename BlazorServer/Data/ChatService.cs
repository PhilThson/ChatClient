using System.Net.Http.Headers;
using BlazorServer.Data.DTOs;
using BlazorServer.Helpers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorServer.Data
{
    public class ChatService
	{
		private readonly HttpClient _httpClient;
        private readonly ProtectedSessionStorage _sessionStore;

        public ChatService(IHttpClientFactory httpClientFactory,
            ProtectedSessionStorage sessionStore)
        {
            _httpClient = httpClientFactory.CreateClient(ChatConstants.HttpChatClient);
            _sessionStore = sessionStore;
        }

        public async Task JoinRoom(int roomId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "room/join/" + roomId);
            await AttachJwt(request);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<ReadRoomDto> GetRoom(int roomId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "room/" + roomId);
            await AttachJwt(request);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReadRoomDto>();
        }

        private async Task AttachJwt(HttpRequestMessage request)
        {
            var result = await _sessionStore.GetAsync<string>(ChatConstants.JwtName);
            var jwt = result.Success ? result.Value : "";
            if (!string.IsNullOrEmpty(jwt))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }
        }
    }
}


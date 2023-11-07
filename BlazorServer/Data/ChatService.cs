using System.Net.Http.Headers;
using BlazorServer.Data.DTOs;
using BlazorServer.Helpers;

namespace BlazorServer.Data
{
    public class ChatService
	{
		private readonly HttpClient _httpClient;

        public ChatService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(ChatConstants.HttpChatClient);
        }

        public async Task<ReadRoomDto> JoinRoom(int roomId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, "room/join/" + roomId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ReadRoomDto>();
        }
    }
}


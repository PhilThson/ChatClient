using Microsoft.AspNetCore.SignalR;

namespace BlazorServer.Hubs;

public class ChatHub : Hub
{
    public Task SendMessage(string user, string message)
    {
        // "ReceiveMessage" - nazwa hooka, na którego ma zostać rozesłane powiadomienie
        return Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}


using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace ChatZone.Chat;

public class ChatHub : Hub
{
    public async Task SendMessage(string groupName, string message)
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        
        await Clients.Group(groupName).SendAsync("Receive", username, message);
    }
    
    public override async Task OnConnectedAsync()
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        if (!string.IsNullOrEmpty(username))
        {
            await Clients.Caller.SendAsync("GetUsername", username);
        }
        
        await base.OnConnectedAsync();
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}
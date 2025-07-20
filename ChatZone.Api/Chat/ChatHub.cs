using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace ChatZone.Chat;

public class ChatHub : Hub
{
    public static ConcurrentDictionary<string, string> UsersGroups = new();
    public static ConcurrentDictionary<string, bool> IsTypeOfChatSingle = new();
    public async Task SendMessage(string groupName, string message)
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        
        await Clients.Group(groupName).SendAsync("Receive", username, message);
    }
    
    public override async Task OnConnectedAsync()
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        if (!string.IsNullOrEmpty(username) && UsersGroups.TryGetValue(username, out var groupName))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        
        await base.OnConnectedAsync();
    }
    
    //get connectionId and add to database while matching 
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
    public object GetPersonGroupAndUsername()
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        var groupName = UsersGroups.GetValueOrDefault(username!);
        var isSingleChat = IsTypeOfChatSingle.FirstOrDefault(x => x.Key == groupName).Value;
        
        return new { username, groupName, isSingleChat};
    }
    
    public async Task LeaveChat(string groupName, bool isSingleChat)
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        UsersGroups.TryRemove(username!, out _);
        IsTypeOfChatSingle.TryRemove(groupName, out _);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        if (isSingleChat)
        {
            var otherPerson = UsersGroups.FirstOrDefault(x => x.Value == groupName && x.Key != username).Key;
            UsersGroups.TryRemove(otherPerson, out _);
            
            await Clients.OthersInGroup(groupName).SendAsync("LeftChat");
        }
    }

    public async Task AddToGroup(int groupName)
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        
        UsersGroups.TryAdd(username!, groupName.ToString());
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName.ToString());
        IsTypeOfChatSingle[username!] = false;
        
        await Clients.Caller.SendAsync("ChatCreated");
    }
}
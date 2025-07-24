using System.Security.Claims;
using ChatZone.Core.Services;
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
            var groupName = ChatGroupStore.GetPersonGroup(username);
            if(groupName is not null) await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
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
        if (username is null) throw new Exception("User does not exist!");
        
        var groupName = ChatGroupStore.GetPersonGroup(username);
        if (groupName is null) throw new Exception("User does not has any group!");
        
        var isSingleChat = ChatGroupStore.IsSingleChat(groupName);
        
        return new { username, groupName, isSingleChat};
    }
    
    public async Task LeaveChat(string groupName, bool isSingleChat)
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        ChatGroupStore.RemovePersonFromGroup(username!);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        if (isSingleChat)
        {
            var otherPerson = ChatGroupStore.GetSecondPersonInSingleChat(groupName, username!);
            
            ChatGroupStore.RemovePersonFromGroup(otherPerson);
            
            await Clients.OthersInGroup(groupName).SendAsync("LeftChat");
        }
    }

    public async Task AddToGroup(int groupName)
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        
        ChatGroupStore.AddPersonToGroup(username!, groupName.ToString());
        ChatGroupStore.AddTypeOfGroup(groupName.ToString(), false);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName.ToString());
        
        await Clients.Caller.SendAsync("ChatCreated");
    }
}
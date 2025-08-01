using System.Security.Claims;
using ChatZone.Core.Services;
using ChatZone.Features.Messages.Add;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatZone.Chat;

public class ChatHub(IMediator mediator) : Hub
{
    public async Task SendMessage(int idGroup, string message, bool isSingleChat)
    {
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        var idPerson = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await mediator.Send(new AddMessageRequest
        {
            Message = message,
            IdSender = int.Parse(idPerson!),
            IdChat = idGroup,
            IsSingleChat = isSingleChat
        });
        
        await Clients.Group(idGroup.ToString()).SendAsync("Receive", username, message);
    }
    
    public override async Task OnConnectedAsync()
    {
        var idPerson = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(idPerson))
        {
            var groupName = ChatGroupStore.GetPersonGroup(int.Parse(idPerson));
            if(groupName is not null) await Groups.AddToGroupAsync(Context.ConnectionId, groupName.ToString()!);
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
        var personId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (personId is null) throw new Exception("User does not exist!");
        
        var username = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
        if (username is null) throw new Exception("Username does not exist!");
        
        var idGroup = ChatGroupStore.GetPersonGroup(int.Parse(personId));
        if (idGroup is null) throw new Exception("User does not has any group!");

        //ToDo IsSingleChat(int? idGroup) добавил ? может быть можно как то обойти?
        var isSingleChat = ChatGroupStore.IsSingleChat(idGroup);
        
        var idPerson = int.Parse(personId);
        var idPartnerPerson = ChatGroupStore.GetSecondPersonInSingleChat(idGroup, int.Parse(personId));
        
        return new { username, idPerson, idGroup, isSingleChat, isPartnerIdPeron = isSingleChat ? idPartnerPerson : (int?)null};
    }
    
    public async Task LeaveChat(int idGroup, bool isSingleChat)
    {
        var idPerson = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ChatGroupStore.RemovePersonFromGroup(int.Parse(idPerson!));
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, idGroup.ToString());

        if (isSingleChat)
        {
            var otherPerson = ChatGroupStore.GetSecondPersonInSingleChat(idGroup, int.Parse(idPerson!));
            
            ChatGroupStore.RemovePersonFromGroup(otherPerson);
            
            await Clients.OthersInGroup(idGroup.ToString()).SendAsync("LeftChat");
        }
    }

    public async Task AddToGroup(int idGroup)
    {
        var idPerson = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        ChatGroupStore.AddPersonToGroup(int.Parse(idPerson!), idGroup);
        ChatGroupStore.AddTypeOfGroup(idGroup, false);
        await Groups.AddToGroupAsync(Context.ConnectionId, idGroup.ToString());
        
        await Clients.Caller.SendAsync("ChatCreated");
    }
}
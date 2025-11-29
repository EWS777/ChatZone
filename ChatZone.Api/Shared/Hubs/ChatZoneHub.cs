using System.Security.Claims;
using ChatZone.Features.Chats.Common.GetActiveChat;
using ChatZone.Features.Messages.Add;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatZone.Shared.Hubs;

public class ChatZoneHub(IMediator mediator) : Hub
{
    public async Task SendMessage(int idGroup, string message, bool isSingleChat)
    {
        if (string.IsNullOrWhiteSpace(message)) throw new HubException("Message is required!");
        if(message.Length >= 1025) throw new HubException("Message max length is 1024 characters!");
        var idSender = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var createdAt = DateTimeOffset.UtcNow;
        await mediator.Send(new AddMessageRequest
        {
            Message = message,
            IdSender = int.Parse(idSender!),
            IdChat = idGroup,
            IsSingleChat = isSingleChat,
            CreatedAt = createdAt
        });
        await Clients.Group(idGroup.ToString()).SendAsync("Receive", int.Parse(idSender!), message, createdAt);
    }
    
    public override async Task OnConnectedAsync()
    {
        var idPerson = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(idPerson))
        {
            var groupName = await mediator.Send(new GetActiveChatRequest { IdPerson = int.Parse(idPerson)});
            if(groupName.IsSingleChat is not null) await Groups.AddToGroupAsync(Context.ConnectionId, groupName.IdChat.ToString() ?? string.Empty);
        }
        
        await base.OnConnectedAsync();
    }
    
    //get connectionId and add to database while matching 
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }

    public async Task AddToGroup(int idGroup)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, idGroup.ToString());
        
        await Clients.Caller.SendAsync("ChatCreated");
    }
}
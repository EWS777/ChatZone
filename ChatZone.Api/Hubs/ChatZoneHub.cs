using System.Security.Claims;
using ChatZone.Features.Chats.Common.GetChat;
using ChatZone.Features.Messages.Add;
using ChatZone.Features.Chats.SingleChats.Finish;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatZone.Hubs;

public class ChatZoneHub(IMediator mediator) : Hub
{
    public async Task SendMessage(int idGroup, string message, bool isSingleChat)
    {
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
            var groupName = await mediator.Send(new GetChatRequest { IdPerson = int.Parse(idPerson)});
            if(groupName is not null) await Groups.AddToGroupAsync(Context.ConnectionId, groupName.Value.ToString());
        }
        
        await base.OnConnectedAsync();
    }
    
    //get connectionId and add to database while matching 
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
    
    public async Task LeaveChat(int idGroup, bool isSingleChat)
    {
        if (isSingleChat)
        {
            await mediator.Send(new FinishSingleChatRequest { IdChat = idGroup });
            await Clients.OthersInGroup(idGroup.ToString()).SendAsync("LeftChat");
        }
    }

    public async Task AddToGroup(int idGroup)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, idGroup.ToString());
        
        await Clients.Caller.SendAsync("ChatCreated");
    }
}
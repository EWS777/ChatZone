using System.Collections.Concurrent;
using System.Security.Claims;
using ChatZone.Features.Chats.Common.GetActiveChat;
using ChatZone.Features.Messages.Add;
using ChatZone.Features.Search.Find;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatZone.Shared.Hubs;

public class ChatZoneHub(IMediator mediator) : Hub
{
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> _activeSearches = new();
    public async Task SendMessage(int idGroup, string message, bool isSingleChat)
    {
        if (string.IsNullOrWhiteSpace(message)) throw new HubException("Message is required!");
        if(message.Length >= 1025) throw new HubException("Message max length is 1024 characters!");
        var idSender = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idSender is null) throw new Exception("User does not exist!");

        var createdAt = DateTimeOffset.UtcNow;
        var result = await mediator.Send(new AddMessageRequest
        {
            Message = message,
            IdSender = int.Parse(idSender),
            IdChat = idGroup,
            IsSingleChat = isSingleChat,
            CreatedAt = createdAt
        });
        if (result.IsSuccess)
        {
            await Clients.Group(idGroup.ToString()).SendAsync("Receive", int.Parse(idSender!), message, createdAt);
        }
        else
        {
            throw new HubException(result.Exception.Message);
        }
    }
    
    public override async Task OnConnectedAsync()
    {
        var idPerson = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(idPerson))
        {
            var groupChatResult = await mediator.Send(new GetActiveChatRequest { IdPerson = int.Parse(idPerson)});
            if (groupChatResult.IsSuccess && groupChatResult.Value.IsSingleChat != null && groupChatResult.Value.IdChat != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupChatResult.Value.IdChat.ToString()!);
            }
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

    public async Task StartSearchSingleChat(FindPersonRequest request)
    {
        var idPerson = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(idPerson)) throw new HubException("User does not exist or not authorized!");

        request.ConnectionId = Context.ConnectionId;
        request.IdPerson = int.Parse(idPerson);

        await Clients.Caller.SendAsync("QueueJoined");
        var result = await mediator.Send(request);
        
        if (!result.IsSuccess) throw new HubException(result.Exception?.Message ?? "An error occurred during search.");
    }
}
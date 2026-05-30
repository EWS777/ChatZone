using System.Collections.Concurrent;
using System.Security.Claims;
using ChatZone.Features.Chats.Common.GetActiveChat;
using ChatZone.Features.GroupMembers.CheckMember;
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
        var idPerson = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var isMember = await mediator.Send(new CheckMemberRequest
        {
            IdChat = idGroup,
            IdPerson = int.Parse(idPerson!)
        });
        
        if (!isMember) throw new HubException("You are not a member of this group!");
        
        await Groups.AddToGroupAsync(Context.ConnectionId, idGroup.ToString());
        await Clients.Caller.SendAsync("ChatCreated");
    }

    public async Task StartSearchSingeChat(FindPersonRequest request)
    {
        var connectionId = Context.ConnectionId;
        request.ConnectionId = connectionId;
        
        var idPerson = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(Context.ConnectionAborted);
        
        //to check if the person exists already
        if (_activeSearches.TryRemove(connectionId, out var oldConnection))
        {
            await oldConnection.CancelAsync();
            oldConnection.Dispose();
        }

        _activeSearches.TryAdd(connectionId, cts);

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var result = await mediator.Send(request, cts.Token);
                
                // if(result.IsSuccess && result.Value.IsFound) break;
            }
            await Task.Delay(3000, cts.Token);
        }
        catch (Exception e)
        {
            //
        }
        finally
        {
            _activeSearches.TryRemove(connectionId, out _);
        }
    }
}
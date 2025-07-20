using ChatApp.Application.DTOs.Message;
using ChatApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatApp.API.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessageToGroup(SendMessageRequestDTO request)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(senderId, out var parsedSenderId))
                return;

            var internalRequest = new SendMessageInternalDTO
            {
                SenderId = parsedSenderId,
                GroupId = request.GroupId,
                Content = request.Content,
                FileUrl = request.FileUrl
            };

            var savedMessage = await _messageService.SendMessageAsync(internalRequest);

            // Send to all users in the group
            if (request.GroupId.HasValue)
                await Clients.Group(request.GroupId.ToString()!).SendAsync("ReceiveGroupMessage", savedMessage);
        }

        public async Task SendMessageToUser(SendMessageRequestDTO request)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(senderId, out var parsedSenderId))
                return;

            var internalRequest = new SendMessageInternalDTO
            {
                SenderId = parsedSenderId,
                ReceiverId = request.ReceiverId,
                Content = request.Content,
                FileUrl = request.FileUrl
            };

            var savedMessage = await _messageService.SendMessageAsync(internalRequest);

            if (request.ReceiverId.HasValue)
                await Clients.User(request.ReceiverId.ToString()!).SendAsync("ReceiveDirectMessage", savedMessage);
        }

        public async Task JoinGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task LeaveGroup(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
    }
}

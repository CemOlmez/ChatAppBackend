using ChatApp.Application.DTOs.Message;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;

        public MessageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MessageDTO> SendMessageAsync(SendMessageInternalDTO request)
        {
            var message = new Message
            {
                SenderId = request.SenderId,
                GroupId = request.GroupId,
                Content = request.Content,
                FileUrl = request.FileUrl,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return new MessageDTO
            {
                Id = message.Id,
                SenderId = message.SenderId,
                GroupId = message.GroupId,
                Content = message.Content,
                FileUrl = message.FileUrl,
                Timestamp = message.Timestamp
            };
        }

        public async Task<List<MessageDTO>> GetMessagesByGroupIdAsync(Guid groupId, int page = 1, int pageSize = 50)
        {
            return await _context.Messages
                .Where(m => m.GroupId == groupId && !m.IsDeleted)
                .OrderByDescending(m => m.Timestamp)
                .Select(m => new MessageDTO
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    GroupId = m.GroupId,
                    Content = m.Content,
                    FileUrl = m.FileUrl,
                    Timestamp = m.Timestamp
                })
                .ToListAsync();
        }

        public async Task<List<MessageDTO>> GetMessagesBetweenUsersAsync(Guid userId1, Guid userId2)
        {
            var messages = await _context.Messages
                .Where(m =>
                    m.Group.IsPrivate &&
                    m.Group.UserGroups.Any(ug => ug.UserId == userId1) &&
                    m.Group.UserGroups.Any(ug => ug.UserId == userId2))
                .OrderByDescending(m => m.Timestamp)
                .Select(m => new MessageDTO
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    GroupId = m.GroupId,
                    Content = m.Content,
                    FileUrl = m.FileUrl,
                    Timestamp = m.Timestamp
                })
                .ToListAsync();

            return messages;
        }
        public async Task<MessageDTO?> UpdateMessageAsync(Guid id, string newContent)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null || message.IsDeleted) return null;

            message.Content = newContent;
            await _context.SaveChangesAsync();

            return new MessageDTO
            {
                Id = message.Id,
                SenderId = message.SenderId,
                GroupId = message.GroupId,
                Content = message.Content,
                FileUrl = message.FileUrl,
                Timestamp = message.Timestamp
            };
        }

        public async Task<bool> DeleteMessageAsync(Guid id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return false;

            message.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

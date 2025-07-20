using ChatApp.Application.DTOs.Message;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.Cache;
using ChatApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cache;

        public MessageService(AppDbContext context, ICacheService cache)
        {

            _context = context;
            _cache = cache;

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

            if (request.GroupId.HasValue)
                await _cache.RemoveAsync($"group:{request.GroupId}");

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
            string cacheKey = $"group:{groupId}:page:{page}:size:{pageSize}";
            var cached = await _cache.GetAsync<List<MessageDTO>>(cacheKey);

            if (cached != null)
                return cached;

            var messages = await _context.Messages
                .Where(m => m.GroupId == groupId && !m.IsDeleted)
                .OrderByDescending(m => m.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

            await _cache.SetAsync(cacheKey, messages, TimeSpan.FromMinutes(5));
            return messages;
        }

        public async Task<List<MessageDTO>> SearchMessagesInGroupAsync(Guid groupId, string keyword)
        {
            return await _context.Messages
                .Where(m => m.GroupId == groupId && !m.IsDeleted && m.Content != null && m.Content.Contains(keyword))
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

            // 🔥 Invalidate cache
            if (message.GroupId.HasValue)
                await _cache.RemoveAsync($"group:{message.GroupId}");

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

            // 🔥 Invalidate cache
            if (message.GroupId.HasValue)
                await _cache.RemoveAsync($"group:{message.GroupId}");

            message.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

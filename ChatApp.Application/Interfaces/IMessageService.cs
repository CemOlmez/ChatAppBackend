using ChatApp.Application.DTOs.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDTO> SendMessageAsync(SendMessageInternalDTO request);
        Task<List<MessageDTO>> GetMessagesByGroupIdAsync(Guid groupId, int page = 1, int pageSize = 50);
        Task<List<MessageDTO>> SearchMessagesInGroupAsync(Guid groupId, string keyword);

        Task<MessageDTO?> UpdateMessageAsync(Guid id, string newContent);
        Task<List<MessageDTO>> GetMessagesBetweenUsersAsync(Guid user1Id, Guid user2Id);
        Task<bool> DeleteMessageAsync(Guid id); // Soft delete
    }
}

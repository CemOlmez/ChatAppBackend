using ChatApp.Application.DTOs;
using ChatApp.Application.DTOs.Group;
using ChatApp.Application.DTOs.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddContactAsync(AddContactRequestDTO request);
        Task<Guid> CreateDirectChatAsync(CreateDirectMessageRequestDTO request);
        Task<bool> AddUserToGroupAsync(AddUserToGroupRequestDTO request);
        Task<List<UserDTO>> GetContactsAsync(Guid userId);
    }

}

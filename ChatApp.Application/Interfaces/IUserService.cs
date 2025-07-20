using ChatApp.Application.DTOs.Group;
using ChatApp.Application.DTOs.Message;
using ChatApp.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<List<UserDTO>> GetContactsAsync(Guid userId);
        Task<bool> AddContactAsync(AddUserToContactsDTO request);
        
    }

}

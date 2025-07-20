using ChatApp.Application.DTOs.Group;
using ChatApp.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Interfaces
{
    public interface IGroupService
    {
        Task<GroupDTO> CreateGroupAsync(CreateGroupRequestDTO request);
        Task<List<GroupDTO>> GetAllGroupsAsync();
        Task<GroupDTO?> GetGroupByIdAsync(Guid id);

        Task<bool> AddUserToGroupAsync(Guid groupId, Guid userId);
        Task<List<UserDTO>> GetUsersInGroupAsync(Guid groupId);


    }
}

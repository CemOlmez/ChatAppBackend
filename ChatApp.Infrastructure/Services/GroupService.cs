using ChatApp.Application.DTOs;
using ChatApp.Application.DTOs.Group;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ChatApp.Infrastructure.Services
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;

        public GroupService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GroupDTO> CreateGroupAsync(CreateGroupRequestDTO request)
        {
            var group = new Group
            {
                Name = request.Name,
                IsPrivate = request.IsPrivate
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            if (request.UserIds != null)
            {
                foreach (var userId in request.UserIds)
                {
                    _context.UserGroups.Add(new UserGroup { UserId = userId, GroupId = group.Id });
                }
                await _context.SaveChangesAsync();
            }

            return new GroupDTO
            {
                Id = group.Id,
                Name = group.Name,
                IsPrivate = group.IsPrivate
            };
        }

        public async Task<List<GroupDTO>> GetAllGroupsAsync()
        {
            return await _context.Groups
                .Select(g => new GroupDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsPrivate = g.IsPrivate
                })
                .ToListAsync();
        }

        public async Task<GroupDTO?> GetGroupByIdAsync(Guid id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return null;

            return new GroupDTO
            {
                Id = group.Id,
                Name = group.Name,
                IsPrivate = group.IsPrivate
            };
        }

        public async Task<bool> AddUserToGroupAsync(Guid groupId, Guid userId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            var user = await _context.Users.FindAsync(userId);

            if (group == null || user == null)
                return false;

            bool exists = await _context.UserGroups.AnyAsync(ug => ug.GroupId == groupId && ug.UserId == userId);
            if (exists)
                return false;

            _context.UserGroups.Add(new UserGroup { GroupId = groupId, UserId = userId });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserDTO>> GetUsersInGroupAsync(Guid groupId)
        {
            return await _context.UserGroups
                .Where(ug => ug.GroupId == groupId)
                .Select(ug => new UserDTO
                {
                    Id = ug.User.Id,
                    Username = ug.User.Username,
                    Email = ug.User.Email
                })
                .ToListAsync();
        }
    }
}

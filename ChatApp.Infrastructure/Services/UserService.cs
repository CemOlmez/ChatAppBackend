using ChatApp.Application.DTOs.Group;
using ChatApp.Application.DTOs.Message;
using ChatApp.Application.DTOs.User;
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
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email
                })
                .ToListAsync();
        }
        public async Task<List<UserDTO>> GetContactsAsync(Guid userId)
        {
            return await _context.UserGroups
                .Where(ug => ug.UserId == userId && ug.Group.IsPrivate)
                .SelectMany(ug => ug.Group.UserGroups)
                .Where(other => other.UserId != userId)
                .Select(other => new UserDTO
                {
                    Id = other.User.Id,
                    Username = other.User.Username,
                    Email = other.User.Email
                })
                .Distinct()
                .ToListAsync();
        }
        public async Task<bool> AddContactAsync(AddUserToContactsDTO request)
        {
            var requester = await _context.Users.FindAsync(request.RequesterId);
            var target = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.TargetUsername);

            if (requester == null || target == null)
                return false;

            var existingGroup = await _context.Groups
                .Where(g => g.IsPrivate)
                .Where(g =>
                    g.UserGroups.Count == 2 &&
                    g.UserGroups.Any(ug => ug.UserId == requester.Id) &&
                    g.UserGroups.Any(ug => ug.UserId == target.Id))
                .FirstOrDefaultAsync();

            if (existingGroup != null)
                return false;

            var group = new Group
            {
                Id = Guid.NewGuid(),
                Name = "DM",
                IsPrivate = true
            };

            _context.Groups.Add(group);

            _context.UserGroups.AddRange(new List<UserGroup>
        {
            new UserGroup { UserId = requester.Id, GroupId = group.Id },
            new UserGroup { UserId = target.Id, GroupId = group.Id }
        });

            await _context.SaveChangesAsync();
            return true;
        }
        
        
    }
}

using ChatApp.Application.DTOs;
using ChatApp.Application.DTOs.Group;
using ChatApp.Application.DTOs.Message;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using ChatApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context; // Inject DbContext to fetch users

        public UserController(IUserService userService, AppDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("add-contact")]
        public async Task<IActionResult> AddContact(AddContactRequestDTO request)
        {
            var success = await _userService.AddContactAsync(request);
            if (!success)
                return BadRequest("Failed to add contact or already added.");

            return Ok("Contact added successfully.");
        }

        [HttpPost("create-direct")]
        public async Task<IActionResult> CreateDirectChat(CreateDirectMessageRequestDTO request)
        {
            var groupId = await _userService.CreateDirectChatAsync(request);
            return Ok(new { GroupId = groupId });
        }

        [HttpPost("add-to-group")]
        public async Task<IActionResult> AddUserToGroup(AddUserToGroupRequestDTO request)
        {
            var success = await _userService.AddUserToGroupAsync(request);
            if (!success)
                return BadRequest("User or group not found, or user already in group.");

            return Ok("User(s) added to group.");
        }

        [HttpGet("{userId}/contacts")]
        public async Task<IActionResult> GetContacts(Guid userId)
        {
            var contacts = await _userService.GetContactsAsync(userId);
            return Ok(contacts);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email
                })
                .ToListAsync();

            return Ok(users);
        }
    }
}

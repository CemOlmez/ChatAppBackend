using ChatApp.Application.DTOs.Group;
using ChatApp.Application.DTOs.Message;
using ChatApp.Application.Interfaces;
using ChatApp.Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ChatApp.Application.DTOs.User;

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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{userId}/contacts")]
        public async Task<IActionResult> GetContacts(Guid userId)
        {
            var contacts = await _userService.GetContactsAsync(userId);
            return Ok(contacts);
        }

        [HttpPost("add-contact")]
        public async Task<IActionResult> AddContact(AddUserToContactsDTO request)
        {
            var success = await _userService.AddContactAsync(request);
            if (!success)
                return BadRequest("Failed to add contact or already added.");

            return Ok("Contact added successfully.");
        }

  

    }
}

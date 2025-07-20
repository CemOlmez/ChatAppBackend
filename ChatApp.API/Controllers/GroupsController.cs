using ChatApp.Application.DTOs.Group;
using ChatApp.Application.Interfaces;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(CreateGroupRequestDTO request)
        {
            var group = await _groupService.CreateGroupAsync(request);
            return Ok(group);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(Guid id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            return group == null ? NotFound() : Ok(group);
        }

        [HttpPost("{groupId}/users/{userId}")]
        public async Task<IActionResult> AddUserToGroup(Guid groupId, Guid userId)
        {
            var success = await _groupService.AddUserToGroupAsync(groupId, userId);
            return success ? Ok("User added to group.") : BadRequest("User already in group or not found.");
        }

        [HttpGet("{groupId}/users")]
        public async Task<IActionResult> GetUsersInGroup(Guid groupId)
        {
            var users = await _groupService.GetUsersInGroupAsync(groupId);
            return Ok(users);
        }
    }


}

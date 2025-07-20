using ChatApp.API.DTOs;
using ChatApp.Application.DTOs.Message;
using ChatApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ChatApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequestDTO request)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid senderId))
            {
                return Unauthorized("Invalid user ID in token.");
            }

            var fullRequest = new SendMessageInternalDTO
            {
                SenderId = senderId,
                ReceiverId = request.ReceiverId,
                GroupId = request.GroupId,
                Content = request.Content,
                FileUrl = request.FileUrl
            };

            var message = await _messageService.SendMessageAsync(fullRequest);
            return Ok(message);
        }

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetMessagesByGroupId(Guid groupId, int page = 1, int pageSize = 50)
        {
            var messages = await _messageService.GetMessagesByGroupIdAsync(groupId, page, pageSize);
            return Ok(messages);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(Guid id, [FromBody] string newContent)
        {
            var updated = await _messageService.UpdateMessageAsync(id, newContent);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var success = await _messageService.DeleteMessageAsync(id);
            return success ? NoContent() : NotFound();
        }


        [HttpPost("upload")]
        [RequestSizeLimit(10_000_000)] // 10MB max (adjust as needed)
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDTO request)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            return Ok(new { fileUrl });
        }

        [HttpGet("group/{groupId}/search")]
        public async Task<IActionResult> SearchMessages(Guid groupId, [FromQuery] string keyword)
        {
            var results = await _messageService.SearchMessagesInGroupAsync(groupId, keyword);
            return Ok(results);
        }



    }

}

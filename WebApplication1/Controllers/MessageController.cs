using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _messageService.GetAll();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var message = await _messageService.GetById(id);
            if (message == null)
            {
                return NotFound($"Message with id {id} not found");
            }
            return Ok(message);
        }

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetByChatId(string chatId)
        {
            var messages = await _messageService.GetByChatId(chatId);
            return Ok(messages);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var messages = await _messageService.GetByUserId(userId);
            return Ok(messages);
        }

        [HttpGet("chat/{chatId}/user/{userId}/unread/count")]
        public async Task<IActionResult> GetUnreadCount(string chatId, string userId)
        {
            var count = await _messageService.GetUnreadCount(chatId, userId);
            return Ok(new { UnreadCount = count });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Message message)
        {
            var createdMessage = await _messageService.Create(message);
            return CreatedAtAction(nameof(GetById), new { id = createdMessage.Id }, createdMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Message message)
        {
            if (id != message.Id.ToString())
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedMessage = await _messageService.Update(message);
            return Ok(updatedMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _messageService.Delete(id);
            return NoContent();
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _messageService.MarkAsRead(id);
            return Ok();
        }

        [HttpPost("chat/{chatId}/user/{userId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(string chatId, string userId)
        {
            await _messageService.MarkAllAsRead(chatId, userId);
            return Ok();
        }
    }
}
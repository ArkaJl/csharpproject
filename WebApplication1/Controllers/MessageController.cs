using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Контроллер для управления сообщениями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        /// <summary>
        /// получить все сообщения
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _messageService.GetAll();
            return Ok(messages);
        }
        /// <summary>
        /// получить сообщение по id
        /// </summary>

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
        /// <summary>
        /// получить сообщения из чата
        /// </summary>

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetByChatId(string chatId)
        {
            var messages = await _messageService.GetByChatId(chatId);
            return Ok(messages);
        }
        /// <summary>
        /// получить сообщения пользователя
        /// </summary>

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var messages = await _messageService.GetByUserId(userId);
            return Ok(messages);
        }
        /// <summary>
        /// получить количество непрочитанных сообщений пользователя в чате
        /// </summary>

        [HttpGet("chat/{chatId}/user/{userId}/unread/count")]
        public async Task<IActionResult> GetUnreadCount(string chatId, string userId)
        {
            var count = await _messageService.GetUnreadCount(chatId, userId);
            return Ok(new { UnreadCount = count });
        }
        /// <summary>
        /// создать сообщение
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Message message)
        {
            var createdMessage = await _messageService.Create(message);
            return CreatedAtAction(nameof(GetById), new { id = createdMessage.Id }, createdMessage);
        }
        /// <summary>
        /// изменить сообщение
        /// </summary>

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
        /// <summary>
        /// удалить сообщение
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _messageService.Delete(id);
            return NoContent();
        }
        /// <summary>
        /// пометить прочитанным сообщение
        /// </summary>

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _messageService.MarkAsRead(id);
            return Ok();
        }
        /// <summary>
        /// пометить все сообщения пользоватея прочитанными в чате
        /// </summary>

        [HttpPost("chat/{chatId}/user/{userId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(string chatId, string userId)
        {
            await _messageService.MarkAllAsRead(chatId, userId);
            return Ok();
        }
    }
}
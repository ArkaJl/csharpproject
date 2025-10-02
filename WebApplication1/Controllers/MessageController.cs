using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Message;

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
            var response = messages.Select(m => new MessageResponse
            {
                Id = m.Id,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                Content = m.Content,
                ReadStatus = m.ReadStatus,
                CreatedAt = m.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить сообщение по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var message = await _messageService.GetById(id.ToString());
            if (message == null)
            {
                return NotFound($"Message with id {id} not found");
            }

            var response = new MessageResponse
            {
                Id = message.Id,
                ChatId = message.ChatId,
                SenderId = message.SenderId,
                Content = message.Content,
                ReadStatus = message.ReadStatus,
                CreatedAt = message.CreatedAt
            };
            return Ok(response);
        }

        /// <summary>
        /// получить сообщения из чата
        /// </summary>
        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetByChatId(Guid chatId)
        {
            var messages = await _messageService.GetByChatId(chatId.ToString());
            var response = messages.Select(m => new MessageResponse
            {
                Id = m.Id,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                Content = m.Content,
                ReadStatus = m.ReadStatus,
                CreatedAt = m.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить сообщения пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var messages = await _messageService.GetByUserId(userId.ToString());
            var response = messages.Select(m => new MessageResponse
            {
                Id = m.Id,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                Content = m.Content,
                ReadStatus = m.ReadStatus,
                CreatedAt = m.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить количество непрочитанных сообщений пользователя в чате
        /// </summary>
        [HttpGet("chat/{chatId}/user/{userId}/unread/count")]
        public async Task<IActionResult> GetUnreadCount(Guid chatId, Guid userId)
        {
            var count = await _messageService.GetUnreadCount(chatId.ToString(), userId.ToString());
            return Ok(new { UnreadCount = count });
        }

        /// <summary>
        /// создать сообщение
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMessageRequest request)
        {
            var message = new Message
            {
                ChatId = request.ChatId,
                SenderId = request.SenderId,
                Content = request.Content,
                ReadStatus = false, // Новые сообщения по умолчанию непрочитанные
                CreatedAt = DateTime.UtcNow
            };

            var createdMessage = await _messageService.Create(message);

            var response = new MessageResponse
            {
                Id = createdMessage.Id,
                ChatId = createdMessage.ChatId,
                SenderId = createdMessage.SenderId,
                Content = createdMessage.Content,
                ReadStatus = createdMessage.ReadStatus,
                CreatedAt = createdMessage.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// изменить сообщение
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateMessageRequest request)
        {
            var existingMessage = await _messageService.GetById(id.ToString());
            if (existingMessage == null)
            {
                return NotFound($"Message with id {id} not found");
            }

            // Обновляем только переданные поля
            if (request.ChatId != Guid.Empty)
                existingMessage.ChatId = request.ChatId;

            if (request.SenderId != Guid.Empty)
                existingMessage.SenderId = request.SenderId;

            if (!string.IsNullOrEmpty(request.Content))
                existingMessage.Content = request.Content;

            var updatedMessage = await _messageService.Update(existingMessage);

            var response = new MessageResponse
            {
                Id = updatedMessage.Id,
                ChatId = updatedMessage.ChatId,
                SenderId = updatedMessage.SenderId,
                Content = updatedMessage.Content,
                ReadStatus = updatedMessage.ReadStatus,
                CreatedAt = updatedMessage.CreatedAt
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить сообщение
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingMessage = await _messageService.GetById(id.ToString());
            if (existingMessage == null)
            {
                return NotFound($"Message with id {id} not found");
            }

            await _messageService.Delete(id.ToString());
            return NoContent();
        }

        /// <summary>
        /// пометить сообщение прочитанным
        /// </summary>
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var existingMessage = await _messageService.GetById(id.ToString());
            if (existingMessage == null)
            {
                return NotFound($"Message with id {id} not found");
            }

            await _messageService.MarkAsRead(id.ToString());
            return Ok(new { message = "Message marked as read" });
        }

        /// <summary>
        /// пометить все сообщения пользователя прочитанными в чате
        /// </summary>
        [HttpPost("chat/{chatId}/user/{userId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(Guid chatId, Guid userId)
        {
            await _messageService.MarkAllAsRead(chatId.ToString(), userId.ToString());
            return Ok(new { message = "All messages marked as read" });
        }

    }
}
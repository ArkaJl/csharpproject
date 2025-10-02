using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Chat;
using WebApplication1.Contracts.Message;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Контроллер для управления чатами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        /// <summary>
        /// получить все чаты
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chats = await _chatService.GetAll();
            var response = chats.Select(c => new ChatResponse
            {
                Id = c.Id,
                CommunityId = c.CommunityId,
                Name = c.Name,
                Type = c.Type,
                CreatedAt = c.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить чат по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var chat = await _chatService.GetById(id.ToString());
            if (chat == null)
            {
                return NotFound($"Chat with id {id} not found");
            }

            var response = new ChatResponse
            {
                Id = chat.Id,
                CommunityId = chat.CommunityId,
                Name = chat.Name,
                Type = chat.Type,
                CreatedAt = chat.CreatedAt
            };
            return Ok(response);
        }

        /// <summary>
        /// получить чаты сообщества
        /// </summary>
        [HttpGet("community/{communityId}")]
        public async Task<IActionResult> GetByCommunityId(Guid communityId)
        {
            var chats = await _chatService.GetByCommunityId(communityId.ToString());
            var response = chats.Select(c => new ChatResponse
            {
                Id = c.Id,
                CommunityId = c.CommunityId,
                Name = c.Name,
                Type = c.Type,
                CreatedAt = c.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить чаты пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var chats = await _chatService.GetByUserId(userId.ToString());
            var response = chats.Select(c => new ChatResponse
            {
                Id = c.Id,
                CommunityId = c.CommunityId,
                Name = c.Name,
                Type = c.Type,
                CreatedAt = c.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// создать чат
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChatRequest request)
        {
            var chat = new Chat
            {
                CommunityId = request.CommunityId,
                Name = request.Name,
                Type = request.Type,
                CreatedAt = DateTime.UtcNow
            };

            var createdChat = await _chatService.Create(chat);
            
            var response = new ChatResponse
            {
                Id = createdChat.Id,
                CommunityId = createdChat.CommunityId,
                Name = createdChat.Name,
                Type = createdChat.Type,
                CreatedAt = createdChat.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// изменить чат
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateChatRequest request)
        {
            var existingChat = await _chatService.GetById(id.ToString());
            if (existingChat == null)
            {
                return NotFound($"Chat with id {id} not found");
            }

            // Обновляем только переданные поля
            if (request.CommunityId.HasValue)
                existingChat.CommunityId = request.CommunityId;
                
            if (!string.IsNullOrEmpty(request.Name))
                existingChat.Name = request.Name;
                
            if (!string.IsNullOrEmpty(request.Type))
                existingChat.Type = request.Type;

            var updatedChat = await _chatService.Update(existingChat);
            
            var response = new ChatResponse
            {
                Id = updatedChat.Id,
                CommunityId = updatedChat.CommunityId,
                Name = updatedChat.Name,
                Type = updatedChat.Type,
                CreatedAt = updatedChat.CreatedAt
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить чат
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingChat = await _chatService.GetById(id.ToString());
            if (existingChat == null)
            {
                return NotFound($"Chat with id {id} not found");
            }

            await _chatService.Delete(id.ToString());
            return NoContent();
        }

        /// <summary>
        /// добавить участника в чат
        /// </summary>
        [HttpPost("{chatId}/participants/{userId}")]
        public async Task<IActionResult> AddParticipant(Guid chatId, Guid userId)
        {
            await _chatService.AddParticipant(chatId.ToString(), userId.ToString());
            return Ok(new { message = "Participant added successfully" });
        }

        /// <summary>
        /// удалить участника чата
        /// </summary>
        [HttpDelete("{chatId}/participants/{userId}")]
        public async Task<IActionResult> RemoveParticipant(Guid chatId, Guid userId)
        {
            await _chatService.RemoveParticipant(chatId.ToString(), userId.ToString());
            return NoContent();
        }

        /// <summary>
        /// получить всех участников чата
        /// </summary>
        [HttpGet("{chatId}/participants")]
        public async Task<IActionResult> GetChatParticipants(Guid chatId)
        {
            var participants = await _chatService.GetChatParticipants(chatId.ToString());
            // Предполагая, что возвращается список пользователей или ChatParticipant
            var response = participants.Select(p => new
            {
                UserId = p.UserId,
                ChatId = p.ChatId,
                JoinedAt = p.JoinedAt,
                LastRead = p.LastRead
            });
            return Ok(response);
        }
    }
}
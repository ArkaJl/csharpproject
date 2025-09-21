using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chats = await _chatService.GetAll();
            return Ok(chats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var chat = await _chatService.GetById(id);
            if (chat == null)
            {
                return NotFound($"Chat with id {id} not found");
            }
            return Ok(chat);
        }

        [HttpGet("community/{communityId}")]
        public async Task<IActionResult> GetByCommunityId(string communityId)
        {
            var chats = await _chatService.GetByCommunityId(communityId);
            return Ok(chats);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var chats = await _chatService.GetByUserId(userId);
            return Ok(chats);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Chat chat)
        {
            var createdChat = await _chatService.Create(chat);
            return CreatedAtAction(nameof(GetById), new { id = createdChat.Id }, createdChat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Chat chat)
        {
            if (id != chat.Id.ToString())
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedChat = await _chatService.Update(chat);
            return Ok(updatedChat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _chatService.Delete(id);
            return NoContent();
        }

        [HttpPost("{chatId}/participants/{userId}")]
        public async Task<IActionResult> AddParticipant(string chatId, string userId)
        {
            await _chatService.AddParticipant(chatId, userId);
            return Ok();
        }

        [HttpDelete("{chatId}/participants/{userId}")]
        public async Task<IActionResult> RemoveParticipant(string chatId, string userId)
        {
            await _chatService.RemoveParticipant(chatId, userId);
            return NoContent();
        }

        [HttpGet("{chatId}/participants")]
        public async Task<IActionResult> GetChatParticipants(string chatId)
        {
            var participants = await _chatService.GetChatParticipants(chatId);
            return Ok(participants);
        }
    }
}
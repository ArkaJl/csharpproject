using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Контроллер для управления сообществами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _communityService;

        public CommunityController(ICommunityService communityService)
        {
            _communityService = communityService;
        }
        /// <summary>
        /// получить все сообщества
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var communities = await _communityService.GetAll();
            return Ok(communities);
        }
        /// <summary>
        /// получить сообщество по id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var community = await _communityService.GetById(id);
            if (community == null)
            {
                return NotFound($"Community with id {id} not found");
            }
            return Ok(community);
        }
        /// <summary>
        /// получить участника сообщества по id
        /// </summary>

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var communities = await _communityService.GetByUserId(userId);
            return Ok(communities);
        }
        /// <summary>
        /// создать сообщество
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Community community)
        {
            var createdCommunity = await _communityService.Create(community);
            return CreatedAtAction(nameof(GetById), new { id = createdCommunity.Id }, createdCommunity);
        }
        /// <summary>
        /// изменить данные сообщества
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Community community)
        {
            if (id != community.Id.ToString())
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedCommunity = await _communityService.Update(community);
            return Ok(updatedCommunity);
        }
        /// <summary>
        /// удалить сообщество
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _communityService.Delete(id);
            return NoContent();
        }
        /// <summary>
        /// добавить участника сообщества
        /// </summary>

        [HttpPost("{communityId}/subscribe/{userId}")]
        public async Task<IActionResult> SubscribeUser(string communityId, string userId, [FromQuery] string role = "member")
        {
            await _communityService.SubscribeUser(communityId, userId, role);
            return Ok();
        }
        /// <summary>
        /// удалить участника сообщества
        /// </summary>

        [HttpDelete("{communityId}/unsubscribe/{userId}")]
        public async Task<IActionResult> UnsubscribeUser(string communityId, string userId)
        {
            await _communityService.UnsubscribeUser(communityId, userId);
            return NoContent();
        }
        /// <summary>
        /// получить всех участников сообщества
        /// </summary>

        [HttpGet("{communityId}/subscribers")]
        public async Task<IActionResult> GetCommunitySubscribers(string communityId)
        {
            var subscribers = await _communityService.GetCommunitySubscribers(communityId);
            return Ok(subscribers);
        }
        /// <summary>
        /// получить подписчиков пользователя
        /// </summary>

        [HttpGet("user/{userId}/subscriptions")]
        public async Task<IActionResult> GetUserSubscriptions(string userId)
        {
            var subscriptions = await _communityService.GetUserSubscriptions(userId);
            return Ok(subscriptions);
        }
        /// <summary>
        /// изменить роль участника сообщества
        /// </summary>

        [HttpPut("{communityId}/members/{userId}/role")]
        public async Task<IActionResult> UpdateMemberRole(string communityId, string userId, [FromBody] string role)
        {
            await _communityService.UpdateMemberRole(communityId, userId, role);
            return Ok();
        }
    }
}
using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Community;
using WebApplication1.Contracts.Subscription;

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
            var response = communities.Select(c => new CommunityResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                AvatarUrl = c.AvatarUrl,
                BannerUrl = c.BannerUrl,
                CreatorId = c.CreatorId,
                CreatedAt = c.CreatedAt,
                Tags = c.Tags,
                MemberCount = c.MemberCount
            });
            return Ok(response);
        }

        /// <summary>
        /// получить сообщество по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var community = await _communityService.GetById(id.ToString());
            if (community == null)
            {
                return NotFound($"Community with id {id} not found");
            }

            var response = new CommunityResponse
            {
                Id = community.Id,
                Name = community.Name,
                Description = community.Description,
                AvatarUrl = community.AvatarUrl,
                BannerUrl = community.BannerUrl,
                CreatorId = community.CreatorId,
                CreatedAt = community.CreatedAt,
                Tags = community.Tags,
                MemberCount = community.MemberCount
            };
            return Ok(response);
        }

        /// <summary>
        /// получить сообщества пользователя по id пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var communities = await _communityService.GetByUserId(userId.ToString());
            var response = communities.Select(c => new CommunityResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                AvatarUrl = c.AvatarUrl,
                BannerUrl = c.BannerUrl,
                CreatorId = c.CreatorId,
                CreatedAt = c.CreatedAt,
                Tags = c.Tags,
                MemberCount = c.MemberCount
            });
            return Ok(response);
        }

        /// <summary>
        /// создать сообщество
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommunityRequest request)
        {
            var community = new Community
            {
                Name = request.Name,
                Description = request.Description,
                AvatarUrl = request.AvatarUrl,
                BannerUrl = request.BannerUrl,
                Tags = request.Tags,
                MemberCount = 0,
                CreatorId = request.creator_id,
                CreatedAt = DateTime.UtcNow
            };

            var createdCommunity = await _communityService.Create(community);

            var response = new CommunityResponse
            {
                Id = createdCommunity.Id,
                Name = createdCommunity.Name,
                Description = createdCommunity.Description,
                AvatarUrl = createdCommunity.AvatarUrl,
                BannerUrl = createdCommunity.BannerUrl,
                CreatorId = createdCommunity.CreatorId,
                CreatedAt = createdCommunity.CreatedAt,
                Tags = createdCommunity.Tags,
                MemberCount = createdCommunity.MemberCount
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// изменить данные сообщества
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommunityRequest request)
        {
            var existingCommunity = await _communityService.GetById(id.ToString());
            if (existingCommunity == null)
            {
                return NotFound($"Community with id {id} not found");
            }

            // Обновляем только переданные поля
            if (!string.IsNullOrEmpty(request.Name))
                existingCommunity.Name = request.Name;

            if (request.Description != null)
                existingCommunity.Description = request.Description;

            if (request.AvatarUrl != null)
                existingCommunity.AvatarUrl = request.AvatarUrl;

            if (request.BannerUrl != null)
                existingCommunity.BannerUrl = request.BannerUrl;

            if (request.Tags != null)
                existingCommunity.Tags = request.Tags;

            var updatedCommunity = await _communityService.Update(existingCommunity);

            var response = new CommunityResponse
            {
                Id = updatedCommunity.Id,
                Name = updatedCommunity.Name,
                Description = updatedCommunity.Description,
                AvatarUrl = updatedCommunity.AvatarUrl,
                BannerUrl = updatedCommunity.BannerUrl,
                CreatorId = updatedCommunity.CreatorId,
                CreatedAt = updatedCommunity.CreatedAt,
                Tags = updatedCommunity.Tags,
                MemberCount = updatedCommunity.MemberCount
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить сообщество
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingCommunity = await _communityService.GetById(id.ToString());
            if (existingCommunity == null)
            {
                return NotFound($"Community with id {id} not found");
            }

            await _communityService.Delete(id.ToString());
            return NoContent();
        }

        /// <summary>
        /// добавить участника сообщества
        /// </summary>
        [HttpPost("{communityId}/subscribe/{userId}")]
        public async Task<IActionResult> SubscribeUser(Guid communityId, Guid userId, [FromQuery] string role = "member")
        {
            var request = new CreateSubscriptionRequest
            {
                UserId = userId,
                CommunityId = communityId,
                Role = role
            };

            await _communityService.SubscribeUser(communityId.ToString(), userId.ToString(), role);
            return Ok(new { message = "User subscribed successfully" });
        }

        /// <summary>
        /// удалить участника сообщества
        /// </summary>
        [HttpDelete("{communityId}/unsubscribe/{userId}")]
        public async Task<IActionResult> UnsubscribeUser(Guid communityId, Guid userId)
        {
            await _communityService.UnsubscribeUser(communityId.ToString(), userId.ToString());
            return NoContent();
        }

        /// <summary>
        /// получить всех участников сообщества
        /// </summary>
        [HttpGet("{communityId}/subscribers")]
        public async Task<IActionResult> GetCommunitySubscribers(Guid communityId)
        {
            var subscribers = await _communityService.GetCommunitySubscribers(communityId.ToString());
            // Предполагая, что возвращается список User или Subscription
            var response = subscribers.Select(s => new SubscriptionResponse
            {
                UserId = s.UserId,
                CommunityId = s.CommunityId,
                Role = s.Role,
                JoinedAt = s.JoinedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить подписки пользователя
        /// </summary>
        [HttpGet("user/{userId}/subscriptions")]
        public async Task<IActionResult> GetUserSubscriptions(Guid userId)
        {
            var subscriptions = await _communityService.GetUserSubscriptions(userId.ToString());
            var response = subscriptions.Select(s => new SubscriptionResponse
            {
                UserId = s.UserId,
                CommunityId = s.CommunityId,
                Role = s.Role,
                JoinedAt = s.JoinedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// изменить роль участника сообщества
        /// </summary>
        [HttpPut("{communityId}/members/{userId}/role")]
        public async Task<IActionResult> UpdateMemberRole(Guid communityId, Guid userId, [FromBody] string role)
        {
            await _communityService.UpdateMemberRole(communityId.ToString(), userId.ToString(), role);
            return Ok(new { message = "Member role updated successfully" });
        }
    }
}
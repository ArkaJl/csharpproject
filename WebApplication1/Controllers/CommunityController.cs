using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _communityService;

        public CommunityController(ICommunityService communityService)
        {
            _communityService = communityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var communities = await _communityService.GetAll();
            return Ok(communities);
        }

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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var communities = await _communityService.GetByUserId(userId);
            return Ok(communities);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Community community)
        {
            var createdCommunity = await _communityService.Create(community);
            return CreatedAtAction(nameof(GetById), new { id = createdCommunity.Id }, createdCommunity);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _communityService.Delete(id);
            return NoContent();
        }

        [HttpPost("{communityId}/subscribe/{userId}")]
        public async Task<IActionResult> SubscribeUser(string communityId, string userId, [FromQuery] string role = "member")
        {
            await _communityService.SubscribeUser(communityId, userId, role);
            return Ok();
        }

        [HttpDelete("{communityId}/unsubscribe/{userId}")]
        public async Task<IActionResult> UnsubscribeUser(string communityId, string userId)
        {
            await _communityService.UnsubscribeUser(communityId, userId);
            return NoContent();
        }

        [HttpGet("{communityId}/subscribers")]
        public async Task<IActionResult> GetCommunitySubscribers(string communityId)
        {
            var subscribers = await _communityService.GetCommunitySubscribers(communityId);
            return Ok(subscribers);
        }

        [HttpGet("user/{userId}/subscriptions")]
        public async Task<IActionResult> GetUserSubscriptions(string userId)
        {
            var subscriptions = await _communityService.GetUserSubscriptions(userId);
            return Ok(subscriptions);
        }

        [HttpPut("{communityId}/members/{userId}/role")]
        public async Task<IActionResult> UpdateMemberRole(string communityId, string userId, [FromBody] string role)
        {
            await _communityService.UpdateMemberRole(communityId, userId, role);
            return Ok();
        }
    }
}
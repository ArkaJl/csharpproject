using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInventoryController : ControllerBase
    {
        private readonly IUserInventoryService _userInventoryService;

        public UserInventoryController(IUserInventoryService userInventoryService)
        {
            _userInventoryService = userInventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userInventoryService.GetAll());
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var userInventory = await _userInventoryService.GetByUserId(userId);
            if (userInventory == null || !userInventory.Any())
            {
                return NotFound($"No inventory items found for user with id {userId}");
            }
            return Ok(userInventory);
        }

        [HttpGet("user/{userId}/item/{itemId}")]
        public async Task<IActionResult> GetByUserAndItemId(string userId, string itemId)
        {
            var userInventory = await _userInventoryService.GetByUserAndItemId(userId, itemId);
            if (userInventory == null)
            {
                return NotFound($"Inventory item not found for user {userId} and item {itemId}");
            }
            return Ok(userInventory);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserInventory userInventory)
        {
            await _userInventoryService.Create(userInventory);
            return CreatedAtAction(nameof(GetByUserAndItemId),
                new { userId = userInventory.UserId, itemId = userInventory.ItemId },
                userInventory);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserInventory userInventory)
        {
            await _userInventoryService.Update(userInventory);
            return Ok();
        }

        [HttpDelete("user/{userId}/item/{itemId}")]
        public async Task<IActionResult> Delete(string userId, string itemId)
        {
            await _userInventoryService.Delete(userId, itemId);
            return NoContent();
        }

        [HttpPost("user/{userId}/item/{itemId}/equip")]
        public async Task<IActionResult> EquipItem(string userId, string itemId)
        {
            await _userInventoryService.EquipItem(userId, itemId);
            return Ok();
        }

        [HttpPost("user/{userId}/item/{itemId}/unequip")]
        public async Task<IActionResult> UnequipItem(string userId, string itemId)
        {
            await _userInventoryService.UnequipItem(userId, itemId);
            return Ok();
        }
    }
}
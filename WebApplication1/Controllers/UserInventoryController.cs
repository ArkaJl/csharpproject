
using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    /// <summary>
    /// контроллер для управления инвентарем пользователя
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserInventoryController : ControllerBase
    {
        private readonly IUserInventoryService _userInventoryService;

        public UserInventoryController(IUserInventoryService userInventoryService)
        {
            _userInventoryService = userInventoryService;
        }
        /// <summary>
        /// получить все инвентари
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userInventoryService.GetAll());
        }
        /// <summary>
        /// получить инвыентарь пользователя
        /// </summary>

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
        /// <summary>
        /// получить предмет пользователя
        /// </summary>

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
        /// <summary>
        /// добавить предмет в инвентарь пользователя
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserInventory userInventory)
        {
            await _userInventoryService.Create(userInventory);
            return CreatedAtAction(nameof(GetByUserAndItemId),
                new { userId = userInventory.UserId, itemId = userInventory.ItemId },
                userInventory);
        }
        /// <summary>
        /// изменить инвентарь
        /// </summary>

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserInventory userInventory)
        {
            await _userInventoryService.Update(userInventory);
            return Ok();
        }
        /// <summary>
        /// удалить предмет из инвентаря пользователя
        /// </summary>

        [HttpDelete("user/{userId}/item/{itemId}")]
        public async Task<IActionResult> Delete(string userId, string itemId)
        {
            await _userInventoryService.Delete(userId, itemId);
            return NoContent();
        }
        /// <summary>
        /// экипировать предмет пользоватеоля
        /// </summary>

        [HttpPost("user/{userId}/item/{itemId}/equip")]
        public async Task<IActionResult> EquipItem(string userId, string itemId)
        {
            await _userInventoryService.EquipItem(userId, itemId);
            return Ok();
        }
        /// <summary>
        /// снять предмет пользователя
        /// </summary>

        [HttpPost("user/{userId}/item/{itemId}/unequip")]
        public async Task<IActionResult> UnequipItem(string userId, string itemId)
        {
            await _userInventoryService.UnequipItem(userId, itemId);
            return Ok();
        }
    }
}
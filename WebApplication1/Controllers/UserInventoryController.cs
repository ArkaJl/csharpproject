using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Inventory;

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
            var inventories = await _userInventoryService.GetAll();
            var response = inventories.Select(i => new UserInventoryResponse
            {
                UserId = i.UserId,
                ItemId = i.ItemId,
                PurchasedAt = i.PurchasedAt,
                IsEquipped = i.IsEquipped
            });
            return Ok(response);
        }

        /// <summary>
        /// получить инвентарь пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var userInventory = await _userInventoryService.GetByUserId(userId.ToString());
            if (userInventory == null || !userInventory.Any())
            {
                return NotFound($"No inventory items found for user with id {userId}");
            }

            var response = userInventory.Select(i => new UserInventoryResponse
            {
                UserId = i.UserId,
                ItemId = i.ItemId,
                PurchasedAt = i.PurchasedAt,
                IsEquipped = i.IsEquipped
            });
            return Ok(response);
        }

        /// <summary>
        /// получить предмет пользователя
        /// </summary>
        [HttpGet("user/{userId}/item/{itemId}")]
        public async Task<IActionResult> GetByUserAndItemId(Guid userId, Guid itemId)
        {
            var userInventory = await _userInventoryService.GetByUserAndItemId(userId.ToString(), itemId.ToString());
            if (userInventory == null)
            {
                return NotFound($"Inventory item not found for user {userId} and item {itemId}");
            }

            var response = new UserInventoryResponse
            {
                UserId = userInventory.UserId,
                ItemId = userInventory.ItemId,
                PurchasedAt = userInventory.PurchasedAt,
                IsEquipped = userInventory.IsEquipped
            };
            return Ok(response);
        }

        /// <summary>
        /// добавить предмет в инвентарь пользователя
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddItemToInventoryRequest request)
        {
            // Проверяем, существует ли уже такой предмет у пользователя
            var existingItem = await _userInventoryService.GetByUserAndItemId(request.UserId.ToString(), request.ItemId.ToString());
            if (existingItem != null)
            {
                return Conflict($"Item {request.ItemId} already exists in user {request.UserId} inventory");
            }

            var userInventory = new UserInventory
            {
                UserId = request.UserId,
                ItemId = request.ItemId,
                PurchasedAt = DateTime.UtcNow,
                IsEquipped = request.IsEquipped
                // Навигационные свойства User и Item не устанавливаем - они заполнятся автоматически при загрузке
            };

            await _userInventoryService.Create(userInventory);

            var response = new UserInventoryResponse
            {
                UserId = userInventory.UserId,
                ItemId = userInventory.ItemId,
                PurchasedAt = userInventory.PurchasedAt,
                IsEquipped = userInventory.IsEquipped
            };

            return CreatedAtAction(nameof(GetByUserAndItemId),
                new { userId = response.UserId, itemId = response.ItemId },
                response);
        }

        /// <summary>
        /// изменить инвентарь
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EquipItemRequest request)
        {
            var existingInventory = await _userInventoryService.GetByUserAndItemId(request.UserId.ToString(), request.ItemId.ToString());
            if (existingInventory == null)
            {
                return NotFound($"Inventory item not found for user {request.UserId} and item {request.ItemId}");
            }

            existingInventory.IsEquipped = request.IsEquipped;

            await _userInventoryService.Update(existingInventory);

            var response = new UserInventoryResponse
            {
                UserId = existingInventory.UserId,
                ItemId = existingInventory.ItemId,
                PurchasedAt = existingInventory.PurchasedAt,
                IsEquipped = existingInventory.IsEquipped
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить предмет из инвентаря пользователя
        /// </summary>
        [HttpDelete("user/{userId}/item/{itemId}")]
        public async Task<IActionResult> Delete(Guid userId, Guid itemId)
        {
            var existingInventory = await _userInventoryService.GetByUserAndItemId(userId.ToString(), itemId.ToString());
            if (existingInventory == null)
            {
                return NotFound($"Inventory item not found for user {userId} and item {itemId}");
            }

            await _userInventoryService.Delete(userId.ToString(), itemId.ToString());
            return NoContent();
        }

        /// <summary>
        /// экипировать предмет пользователя
        /// </summary>
        [HttpPost("user/{userId}/item/{itemId}/equip")]
        public async Task<IActionResult> EquipItem(Guid userId, Guid itemId)
        {
            var existingInventory = await _userInventoryService.GetByUserAndItemId(userId.ToString(), itemId.ToString());
            if (existingInventory == null)
            {
                return NotFound($"Inventory item not found for user {userId} and item {itemId}");
            }

            await _userInventoryService.EquipItem(userId.ToString(), itemId.ToString());
            return Ok(new { message = "Item equipped successfully" });
        }

        /// <summary>
        /// снять предмет пользователя
        /// </summary>
        [HttpPost("user/{userId}/item/{itemId}/unequip")]
        public async Task<IActionResult> UnequipItem(Guid userId, Guid itemId)
        {
            var existingInventory = await _userInventoryService.GetByUserAndItemId(userId.ToString(), itemId.ToString());
            if (existingInventory == null)
            {
                return NotFound($"Inventory item not found for user {userId} and item {itemId}");
            }

            await _userInventoryService.UnequipItem(userId.ToString(), itemId.ToString());
            return Ok(new { message = "Item unequipped successfully" });
        }

    }
}
using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Store;

namespace BackendApi.Controllers
{
    /// <summary>
    /// контроллер для управления товарами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StoreItemController : ControllerBase
    {
        private readonly IStoreItemService _storeItemService;

        public StoreItemController(IStoreItemService storeItemService)
        {
            _storeItemService = storeItemService;
        }

        /// <summary>
        /// получить все товары
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var storeItems = await _storeItemService.GetAll();
            var response = storeItems.Select(item => new StoreItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Type = item.Type,
                Price = item.Price,
                Thumbnail = item.Thumbnail,
                Description = item.Description,
                Category = item.Category
            });
            return Ok(response);
        }

        /// <summary>
        /// получить товар по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var storeItem = await _storeItemService.GetById(id.ToString());
            if (storeItem == null)
            {
                return NotFound($"Store item with id {id} not found");
            }

            var response = new StoreItemResponse
            {
                Id = storeItem.Id,
                Name = storeItem.Name,
                Type = storeItem.Type,
                Price = storeItem.Price,
                Thumbnail = storeItem.Thumbnail,
                Description = storeItem.Description,
                Category = storeItem.Category
            };
            return Ok(response);
        }

        /// <summary>
        /// получить все товары типа
        /// </summary>
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            // Валидация типа товара
            var validTypes = new[] { "avatar_frame", "sticker_pack", "profile_background", "other" };
            if (!validTypes.Contains(type.ToLower()))
            {
                return BadRequest($"Type must be one of: {string.Join(", ", validTypes)}");
            }

            var storeItems = await _storeItemService.GetByType(type);
            var response = storeItems.Select(item => new StoreItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Type = item.Type,
                Price = item.Price,
                Thumbnail = item.Thumbnail,
                Description = item.Description,
                Category = item.Category
            });
            return Ok(response);
        }

        /// <summary>
        /// получить товары категории
        /// </summary>
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("Category is required");
            }

            var storeItems = await _storeItemService.GetByCategory(category);
            var response = storeItems.Select(item => new StoreItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Type = item.Type,
                Price = item.Price,
                Thumbnail = item.Thumbnail,
                Description = item.Description,
                Category = item.Category
            });
            return Ok(response);
        }

        /// <summary>
        /// получить товары поиска
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("Search term is required");
            }

            var storeItems = await _storeItemService.Search(term);
            var response = storeItems.Select(item => new StoreItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Type = item.Type,
                Price = item.Price,
                Thumbnail = item.Thumbnail,
                Description = item.Description,
                Category = item.Category
            });
            return Ok(response);
        }

        /// <summary>
        /// получить товары по цене
        /// </summary>
        [HttpGet("price-range")]
        public async Task<IActionResult> GetByPriceRange([FromQuery] int minPrice, [FromQuery] int maxPrice)
        {
            if (minPrice < 0)
            {
                return BadRequest("Minimum price cannot be negative");
            }

            if (maxPrice < minPrice)
            {
                return BadRequest("Maximum price cannot be less than minimum price");
            }

            var storeItems = await _storeItemService.GetByPriceRange(minPrice, maxPrice);
            var response = storeItems.Select(item => new StoreItemResponse
            {
                Id = item.Id,
                Name = item.Name,
                Type = item.Type,
                Price = item.Price,
                Thumbnail = item.Thumbnail,
                Description = item.Description,
                Category = item.Category
            });
            return Ok(response);
        }

        /// <summary>
        /// создать товар
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStoreItemRequest request)
        {
            // Валидация типа товара
            var validTypes = new[] { "avatar_frame", "sticker_pack", "profile_background", "other" };
            if (!validTypes.Contains(request.Type.ToLower()))
            {
                return BadRequest($"Type must be one of: {string.Join(", ", validTypes)}");
            }

            if (request.Price < 0)
            {
                return BadRequest("Price cannot be negative");
            }

            var storeItem = new StoreItem
            {
                Name = request.Name,
                Type = request.Type.ToLower(),
                Price = request.Price,
                Thumbnail = request.Thumbnail,
                Description = request.Description,
                Category = request.Category
            };

            var createdStoreItem = await _storeItemService.Create(storeItem);

            var response = new StoreItemResponse
            {
                Id = createdStoreItem.Id,
                Name = createdStoreItem.Name,
                Type = createdStoreItem.Type,
                Price = createdStoreItem.Price,
                Thumbnail = createdStoreItem.Thumbnail,
                Description = createdStoreItem.Description,
                Category = createdStoreItem.Category
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// изменить товар
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateStoreItemRequest request)
        {
            var existingStoreItem = await _storeItemService.GetById(id.ToString());
            if (existingStoreItem == null)
            {
                return NotFound($"Store item with id {id} not found");
            }

            // Валидация типа товара если передан
            if (!string.IsNullOrEmpty(request.Type))
            {
                var validTypes = new[] { "avatar_frame", "sticker_pack", "profile_background", "other" };
                if (!validTypes.Contains(request.Type.ToLower()))
                {
                    return BadRequest($"Type must be one of: {string.Join(", ", validTypes)}");
                }
                existingStoreItem.Type = request.Type.ToLower();
            }

            // Валидация цены
            if (request.Price < 0)
            {
                return BadRequest("Price cannot be negative");
            }

            // Обновляем только переданные поля
            if (!string.IsNullOrEmpty(request.Name))
                existingStoreItem.Name = request.Name;

            if (request.Price >= 0)
                existingStoreItem.Price = request.Price;

            if (request.Thumbnail != null)
                existingStoreItem.Thumbnail = request.Thumbnail;

            if (request.Description != null)
                existingStoreItem.Description = request.Description;

            if (request.Category != null)
                existingStoreItem.Category = request.Category;

            var updatedStoreItem = await _storeItemService.Update(existingStoreItem);

            var response = new StoreItemResponse
            {
                Id = updatedStoreItem.Id,
                Name = updatedStoreItem.Name,
                Type = updatedStoreItem.Type,
                Price = updatedStoreItem.Price,
                Thumbnail = updatedStoreItem.Thumbnail,
                Description = updatedStoreItem.Description,
                Category = updatedStoreItem.Category
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить товар
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingStoreItem = await _storeItemService.GetById(id.ToString());
            if (existingStoreItem == null)
            {
                return NotFound($"Store item with id {id} not found");
            }

            await _storeItemService.Delete(id.ToString());
            return NoContent();
        }
    }
}
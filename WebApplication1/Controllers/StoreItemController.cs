using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreItemController : ControllerBase
    {
        private readonly IStoreItemService _storeItemService;

        public StoreItemController(IStoreItemService storeItemService)
        {
            _storeItemService = storeItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var storeItems = await _storeItemService.GetAll();
            return Ok(storeItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var storeItem = await _storeItemService.GetById(id);
            if (storeItem == null)
            {
                return NotFound($"Store item with id {id} not found");
            }
            return Ok(storeItem);
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var storeItems = await _storeItemService.GetByType(type);
            return Ok(storeItems);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            var storeItems = await _storeItemService.GetByCategory(category);
            return Ok(storeItems);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var storeItems = await _storeItemService.Search(term);
            return Ok(storeItems);
        }

        [HttpGet("price-range")]
        public async Task<IActionResult> GetByPriceRange([FromQuery] int minPrice, [FromQuery] int maxPrice)
        {
            var storeItems = await _storeItemService.GetByPriceRange(minPrice, maxPrice);
            return Ok(storeItems);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StoreItem storeItem)
        {
            var createdStoreItem = await _storeItemService.Create(storeItem);
            return CreatedAtAction(nameof(GetById), new { id = createdStoreItem.Id }, createdStoreItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] StoreItem storeItem)
        {
            if (id != storeItem.Id.ToString())
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedStoreItem = await _storeItemService.Update(storeItem);
            return Ok(updatedStoreItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _storeItemService.Delete(id);
            return NoContent();
        }
    }
}
using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> Add(UserInventory UserInventory)
        {
            await _userInventoryService.Create(UserInventory);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserInventory UserInventory)
        {
            await _userInventoryService.Update(UserInventory);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userInventoryService.Delete(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var UserInventory = await _userInventoryService.GetById(id);
            if (UserInventory == null)
            {
                return NotFound($"UserInventory with id {id} not found");
            }
            return Ok(UserInventory);
        }
    }
}
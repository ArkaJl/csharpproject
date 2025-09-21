using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAll());
        }

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST /User
        ///     {
        ///         "username": "johndoe",
        ///         "email": "john@example.com",
        ///         "passwordHash": "hashed_password",
        ///         "avatarUrl": "https://example.com/avatar.jpg",
        ///         "coins": 100,
        ///         "status": "Online"
        ///     }
        ///
        /// </remarks>
        /// <param name="user">Данные пользователя</param>
        /// <returns>Созданный пользователь</returns>
        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            await _userService.Create(user);
            return Ok();
        }
        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <param name="user">Обновленные данные</param>
        /// <returns>Обновленный пользователь</returns>
        [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
            await _userService.Update(user);
            return Ok();
        }
        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Статус операции</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.Delete(id);
            return NoContent();
        }
        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Данные пользователя</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound($"User with id {id} not found");
            }
            return Ok(user);
        }
    }
}
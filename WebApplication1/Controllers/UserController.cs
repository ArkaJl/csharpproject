using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using WebApplication1.Contracts.User;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

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
            var users = await _userService.GetAll();
            var response = users.Select(u => new UserResponse
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                AvatarUrl = u.AvatarUrl,
                Coins = u.Coins,
                Status = u.Status,
                CreatedAt = u.CreatedAt,
                LastOnline = u.LastOnline,
                ThemePreference = u.ThemePreference
            });
            return Ok(response);
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
        ///         "password": "password123",
        ///         "avatarUrl": "https://example.com/avatar.jpg",
        ///         "status": "Online",
        ///         "themePreference": "dark"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Данные пользователя</param>
        /// <returns>Созданный пользователь</returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.Password, // Хеширование пароля
                AvatarUrl = request.AvatarUrl,
                Status = request.Status,
                ThemePreference = request.ThemePreference,
                Coins = 0, // По умолчанию
                CreatedAt = DateTime.UtcNow,
                LastOnline = DateTime.UtcNow
            };

            await _userService.Create(user);

            var response = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Coins = user.Coins,
                Status = user.Status,
                CreatedAt = user.CreatedAt,
                LastOnline = user.LastOnline,
                ThemePreference = user.ThemePreference
            };

            return Ok(response);
        }

        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <param name="request">Обновленные данные</param>
        /// <returns>Обновленный пользователь</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
        {
            var existingUser = await _userService.GetById(id.ToString());
            if (existingUser == null)
            {
                return NotFound($"User with id {id} not found");
            }

            // Обновляем только переданные поля
            if (!string.IsNullOrEmpty(request.Username))
                existingUser.Username = request.Username;

            if (!string.IsNullOrEmpty(request.Email))
                existingUser.Email = request.Email;

            if (request.AvatarUrl != null)
                existingUser.AvatarUrl = request.AvatarUrl;

            if (request.Status != null)
                existingUser.Status = request.Status;

            if (!string.IsNullOrEmpty(request.ThemePreference))
                existingUser.ThemePreference = request.ThemePreference;

            await _userService.Update(existingUser);

            var response = new UserResponse
            {
                Id = existingUser.Id,
                Username = existingUser.Username,
                Email = existingUser.Email,
                AvatarUrl = existingUser.AvatarUrl,
                Coins = existingUser.Coins,
                Status = existingUser.Status,
                CreatedAt = existingUser.CreatedAt,
                LastOnline = existingUser.LastOnline,
                ThemePreference = existingUser.ThemePreference
            };

            return Ok(response);
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Статус операции</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingUser = await _userService.GetById(id.ToString());
            if (existingUser == null)
            {
                return NotFound($"User with id {id} not found");
            }

            await _userService.Delete(id.ToString());
            return NoContent();
        }

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Данные пользователя</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id.ToString());
            if (user == null)
            {
                return NotFound($"User with id {id} not found");
            }

            var response = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Coins = user.Coins,
                Status = user.Status,
                CreatedAt = user.CreatedAt,
                LastOnline = user.LastOnline,
                ThemePreference = user.ThemePreference
            };

            return Ok(response);
        }
    }
}
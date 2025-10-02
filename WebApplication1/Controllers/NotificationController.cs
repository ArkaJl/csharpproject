using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Notification;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Контроллер для управления уведомлениями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// получить все уведомления
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetAll();
            var response = notifications.Select(n => new NotificationResponse
            {
                Id = n.Id,
                UserId = n.UserId,
                Type = n.Type,
                SourceId = n.SourceId,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить уведомление по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var notification = await _notificationService.GetById(id.ToString());
            if (notification == null)
            {
                return NotFound($"Notification with id {id} not found");
            }

            var response = new NotificationResponse
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Type = notification.Type,
                SourceId = notification.SourceId,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
            return Ok(response);
        }

        /// <summary>
        /// получить уведомления пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var notifications = await _notificationService.GetByUserId(userId.ToString());
            var response = notifications.Select(n => new NotificationResponse
            {
                Id = n.Id,
                UserId = n.UserId,
                Type = n.Type,
                SourceId = n.SourceId,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить непрочитанные уведомления пользователя
        /// </summary>
        [HttpGet("user/{userId}/unread")]
        public async Task<IActionResult> GetUnreadByUserId(Guid userId)
        {
            var notifications = await _notificationService.GetUnreadByUserId(userId.ToString());
            var response = notifications.Select(n => new NotificationResponse
            {
                Id = n.Id,
                UserId = n.UserId,
                Type = n.Type,
                SourceId = n.SourceId,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить количество непрочитанных уведомлений пользователя
        /// </summary>
        [HttpGet("user/{userId}/unread/count")]
        public async Task<IActionResult> GetUnreadCount(Guid userId)
        {
            var count = await _notificationService.GetUnreadCount(userId.ToString());
            return Ok(new { UnreadCount = count });
        }

        /// <summary>
        /// создать уведомление
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request)
        {
            // Валидация типа уведомления
            var validTypes = new[] { "like", "comment", "mention", "system", "new_message" };
            if (!validTypes.Contains(request.Type.ToLower()))
            {
                return BadRequest($"Type must be one of: {string.Join(", ", validTypes)}");
            }

            var notification = new Notification
            {
                UserId = request.UserId,
                Type = request.Type.ToLower(),
                SourceId = request.SourceId,
                IsRead = false, // Новые уведомления по умолчанию непрочитанные
                CreatedAt = DateTime.UtcNow
            };

            var createdNotification = await _notificationService.Create(notification);

            var response = new NotificationResponse
            {
                Id = createdNotification.Id,
                UserId = createdNotification.UserId,
                Type = createdNotification.Type,
                SourceId = createdNotification.SourceId,
                IsRead = createdNotification.IsRead,
                CreatedAt = createdNotification.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// изменить уведомление
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateNotificationRequest request)
        {
            var existingNotification = await _notificationService.GetById(id.ToString());
            if (existingNotification == null)
            {
                return NotFound($"Notification with id {id} not found");
            }

            // Валидация типа уведомления если передан
            if (!string.IsNullOrEmpty(request.Type))
            {
                var validTypes = new[] { "like", "comment", "mention", "system", "new_message" };
                if (!validTypes.Contains(request.Type.ToLower()))
                {
                    return BadRequest($"Type must be one of: {string.Join(", ", validTypes)}");
                }
                existingNotification.Type = request.Type.ToLower();
            }

            // Обновляем только переданные поля
            if (request.UserId != Guid.Empty)
                existingNotification.UserId = request.UserId;

            if (request.SourceId.HasValue)
                existingNotification.SourceId = request.SourceId;

            var updatedNotification = await _notificationService.Update(existingNotification);

            var response = new NotificationResponse
            {
                Id = updatedNotification.Id,
                UserId = updatedNotification.UserId,
                Type = updatedNotification.Type,
                SourceId = updatedNotification.SourceId,
                IsRead = updatedNotification.IsRead,
                CreatedAt = updatedNotification.CreatedAt
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить уведомление
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingNotification = await _notificationService.GetById(id.ToString());
            if (existingNotification == null)
            {
                return NotFound($"Notification with id {id} not found");
            }

            await _notificationService.Delete(id.ToString());
            return NoContent();
        }

        /// <summary>
        /// пометить уведомление прочитанным
        /// </summary>
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var existingNotification = await _notificationService.GetById(id.ToString());
            if (existingNotification == null)
            {
                return NotFound($"Notification with id {id} not found");
            }

            await _notificationService.MarkAsRead(id.ToString());
            return Ok(new { message = "Notification marked as read" });
        }

        /// <summary>
        /// пометить все уведомления прочитанными пользователя
        /// </summary>
        [HttpPost("user/{userId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(Guid userId)
        {
            await _notificationService.MarkAllAsRead(userId.ToString());
            return Ok(new { message = "All notifications marked as read" });
        }
    }
}
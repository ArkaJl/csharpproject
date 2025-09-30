using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(notifications);
        }
        /// <summary>
        /// получить уведомление по id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var notification = await _notificationService.GetById(id);
            if (notification == null)
            {
                return NotFound($"Notification with id {id} not found");
            }
            return Ok(notification);
        }
        /// <summary>
        /// получить уведомления пользователя
        /// </summary>

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var notifications = await _notificationService.GetByUserId(userId);
            return Ok(notifications);
        }
        /// <summary>
        /// получить непрочитанные уведомления пользователя
        /// </summary>

        [HttpGet("user/{userId}/unread")]
        public async Task<IActionResult> GetUnreadByUserId(string userId)
        {
            var notifications = await _notificationService.GetUnreadByUserId(userId);
            return Ok(notifications);
        }
        /// <summary>
        /// получить количество непрочитанных сообщений пользователя
        /// </summary>

        [HttpGet("user/{userId}/unread/count")]
        public async Task<IActionResult> GetUnreadCount(string userId)
        {
            var count = await _notificationService.GetUnreadCount(userId);
            return Ok(new { UnreadCount = count });
        }
        /// <summary>
        /// создать уведомление
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notification notification)
        {
            var createdNotification = await _notificationService.Create(notification);
            return CreatedAtAction(nameof(GetById), new { id = createdNotification.Id }, createdNotification);
        }
        /// <summary>
        /// изменить уведомление
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Notification notification)
        {
            if (id != notification.Id.ToString())
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedNotification = await _notificationService.Update(notification);
            return Ok(updatedNotification);
        }
        /// <summary>
        /// удалить уведомление
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _notificationService.Delete(id);
            return NoContent();
        }
        /// <summary>
        /// пометить уведомление прочитанным
        /// </summary>

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _notificationService.MarkAsRead(id);
            return Ok();
        }
        /// <summary>
        /// пометить все уведомления прочитанными пользователя
        /// </summary>

        [HttpPost("user/{userId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(string userId)
        {
            await _notificationService.MarkAllAsRead(userId);
            return Ok();
        }
    }
}
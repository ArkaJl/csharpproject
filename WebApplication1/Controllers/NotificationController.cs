using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _notificationService.GetAll();
            return Ok(notifications);
        }

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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var notifications = await _notificationService.GetByUserId(userId);
            return Ok(notifications);
        }

        [HttpGet("user/{userId}/unread")]
        public async Task<IActionResult> GetUnreadByUserId(string userId)
        {
            var notifications = await _notificationService.GetUnreadByUserId(userId);
            return Ok(notifications);
        }

        [HttpGet("user/{userId}/unread/count")]
        public async Task<IActionResult> GetUnreadCount(string userId)
        {
            var count = await _notificationService.GetUnreadCount(userId);
            return Ok(new { UnreadCount = count });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Notification notification)
        {
            var createdNotification = await _notificationService.Create(notification);
            return CreatedAtAction(nameof(GetById), new { id = createdNotification.Id }, createdNotification);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _notificationService.Delete(id);
            return NoContent();
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _notificationService.MarkAsRead(id);
            return Ok();
        }

        [HttpPost("user/{userId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(string userId)
        {
            await _notificationService.MarkAllAsRead(userId);
            return Ok();
        }
    }
}
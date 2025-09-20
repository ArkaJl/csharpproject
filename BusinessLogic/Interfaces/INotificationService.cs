using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetAll();
        Task<Notification?> GetById(string id);
        Task<List<Notification>> GetByUserId(string userId);
        Task<List<Notification>> GetUnreadByUserId(string userId);
        Task<Notification> Create(Notification notification);
        Task<Notification> Update(Notification notification);
        Task Delete(string id);
        Task MarkAsRead(string id);
        Task MarkAllAsRead(string userId);
        Task<int> GetUnreadCount(string userId);
    }
}
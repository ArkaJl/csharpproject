using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public NotificationService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Notification>> GetAll()
        {
            return await _repositoryWrapper.Notification.FindAll()
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification?> GetById(string id)
        {
            return await _repositoryWrapper.Notification
                .FindByCondition(n => n.Id.ToString() == id)
                .Include(n => n.User)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Notification>> GetByUserId(string userId)
        {
            return await _repositoryWrapper.Notification
                .FindByCondition(n => n.UserId.ToString() == userId)
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadByUserId(string userId)
        {
            return await _repositoryWrapper.Notification
                .FindByCondition(n => n.UserId.ToString() == userId && !n.IsRead)
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification> Create(Notification notification)
        {
            _repositoryWrapper.Notification.Create(notification);
            await _repositoryWrapper.SaveAsync();
            return notification;
        }

        public async Task<Notification> Update(Notification notification)
        {
            _repositoryWrapper.Notification.Update(notification);
            await _repositoryWrapper.SaveAsync();
            return notification;
        }

        public async Task Delete(string id)
        {
            var notification = await _repositoryWrapper.Notification
                .FindByCondition(n => n.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (notification != null)
            {
                _repositoryWrapper.Notification.Delete(notification);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task MarkAsRead(string id)
        {
            var notification = await _repositoryWrapper.Notification
                .FindByCondition(n => n.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (notification != null)
            {
                notification.IsRead = true;
                _repositoryWrapper.Notification.Update(notification);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task MarkAllAsRead(string userId)
        {
            var notifications = await _repositoryWrapper.Notification
                .FindByCondition(n => n.UserId.ToString() == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                _repositoryWrapper.Notification.Update(notification);
            }

            await _repositoryWrapper.SaveAsync();
        }

        public async Task<int> GetUnreadCount(string userId)
        {
            return await _repositoryWrapper.Notification
                .FindByCondition(n => n.UserId.ToString() == userId && !n.IsRead)
                .CountAsync();
        }
    }
}
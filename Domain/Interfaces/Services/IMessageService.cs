using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IMessageService
    {
        Task<List<Message>> GetAll();
        Task<Message?> GetById(string id);
        Task<List<Message>> GetByChatId(string chatId);
        Task<List<Message>> GetByUserId(string userId);
        Task<Message> Create(Message message);
        Task<Message> Update(Message message);
        Task Delete(string id);
        Task MarkAsRead(string id);
        Task MarkAllAsRead(string chatId, string userId);
        Task<int> GetUnreadCount(string chatId, string userId);
    }
}
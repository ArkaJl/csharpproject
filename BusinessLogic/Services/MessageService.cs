using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public MessageService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Message>> GetAll()
        {
            return await _repositoryWrapper.Message.FindAll()
                .Include(m => m.Sender)
                .Include(m => m.Chat)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Message?> GetById(string id)
        {
            return await _repositoryWrapper.Message
                .FindByCondition(m => m.Id.ToString() == id)
                .Include(m => m.Sender)
                .Include(m => m.Chat)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Message>> GetByChatId(string chatId)
        {
            return await _repositoryWrapper.Message
                .FindByCondition(m => m.ChatId.ToString() == chatId)
                .Include(m => m.Sender)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Message>> GetByUserId(string userId)
        {
            return await _repositoryWrapper.Message
                .FindByCondition(m => m.SenderId.ToString() == userId)
                .Include(m => m.Sender)
                .Include(m => m.Chat)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Message> Create(Message message)
        {
            _repositoryWrapper.Message.Create(message);
            await _repositoryWrapper.SaveAsync();
            return message;
        }

        public async Task<Message> Update(Message message)
        {
            _repositoryWrapper.Message.Update(message);
            await _repositoryWrapper.SaveAsync();
            return message;
        }

        public async Task Delete(string id)
        {
            var message = await _repositoryWrapper.Message
                .FindByCondition(m => m.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (message != null)
            {
                _repositoryWrapper.Message.Delete(message);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task MarkAsRead(string id)
        {
            var message = await _repositoryWrapper.Message
                .FindByCondition(m => m.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (message != null)
            {
                message.ReadStatus = true;
                _repositoryWrapper.Message.Update(message);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task MarkAllAsRead(string chatId, string userId)
        {
            var messages = await _repositoryWrapper.Message
                .FindByCondition(m => m.ChatId.ToString() == chatId &&
                                    m.SenderId.ToString() != userId &&
                                    !m.ReadStatus)
                .ToListAsync();

            foreach (var message in messages)
            {
                message.ReadStatus = true;
                _repositoryWrapper.Message.Update(message);
            }

            await _repositoryWrapper.SaveAsync();
        }

        public async Task<int> GetUnreadCount(string chatId, string userId)
        {
            return await _repositoryWrapper.Message
                .FindByCondition(m => m.ChatId.ToString() == chatId &&
                                    m.SenderId.ToString() != userId &&
                                    !m.ReadStatus)
                .CountAsync();
        }
    }
}
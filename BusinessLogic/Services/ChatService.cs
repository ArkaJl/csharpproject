
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ChatService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Chat>> GetAll()
        {
            return await _repositoryWrapper.Chat.FindAll()
                .Include(c => c.Community)
                .Include(c => c.ChatParticipants)
                .Include(c => c.Messages)
                .ToListAsync();
        }

        public async Task<Chat?> GetById(string id)
        {
            return await _repositoryWrapper.Chat
                .FindByCondition(c => c.Id.ToString() == id)
                .Include(c => c.Community)
                .Include(c => c.ChatParticipants)
                .ThenInclude(cp => cp.User)
                .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Chat>> GetByCommunityId(string communityId)
        {
            return await _repositoryWrapper.Chat
                .FindByCondition(c => c.CommunityId.ToString() == communityId)
                .Include(c => c.Community)
                .Include(c => c.ChatParticipants)
                .Include(c => c.Messages)
                .ToListAsync();
        }

        public async Task<List<Chat>> GetByUserId(string userId)
        {
            return await _repositoryWrapper.Chat
                .FindByCondition(c => c.ChatParticipants.Any(cp => cp.UserId.ToString() == userId))
                .Include(c => c.Community)
                .Include(c => c.ChatParticipants)
                .ThenInclude(cp => cp.User)
                .Include(c => c.Messages)
                .ToListAsync();
        }

        public async Task<Chat> Create(Chat chat)
        {
            await _repositoryWrapper.Chat.Create(chat);
            await _repositoryWrapper.SaveAsync();
            return chat;
        }

        public async Task<Chat> Update(Chat chat)
        {
            await _repositoryWrapper.Chat.Update(chat);
            await _repositoryWrapper.SaveAsync();
            return chat;
        }

        public async Task Delete(string id)
        {
            var chat = await _repositoryWrapper.Chat
                .FindByCondition(c => c.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (chat != null)
            {
                await _repositoryWrapper.Chat.Delete(chat);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task AddParticipant(string chatId, string userId)
        {
            var participant = new ChatParticipant
            {
                ChatId = Guid.Parse(chatId),
                UserId = Guid.Parse(userId),
                JoinedAt = DateTime.UtcNow
            };

            await _repositoryWrapper.ChatParticipant.Create(participant);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task RemoveParticipant(string chatId, string userId)
        {
            var participant = await _repositoryWrapper.ChatParticipant
                .FindByCondition(cp => cp.ChatId.ToString() == chatId && cp.UserId.ToString() == userId)
                .FirstOrDefaultAsync();

            if (participant != null)
            {
                await _repositoryWrapper.ChatParticipant.Delete(participant);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task<List<ChatParticipant>> GetChatParticipants(string chatId)
        {
            return await _repositoryWrapper.ChatParticipant
                .FindByCondition(cp => cp.ChatId.ToString() == chatId)
                .Include(cp => cp.User)
                .ToListAsync();
        }
    }
}
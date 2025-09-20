using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface IChatService
    {
        Task<List<Chat>> GetAll();
        Task<Chat?> GetById(string id);
        Task<List<Chat>> GetByCommunityId(string communityId);
        Task<List<Chat>> GetByUserId(string userId);
        Task<Chat> Create(Chat chat);
        Task<Chat> Update(Chat chat);
        Task Delete(string id);
        Task AddParticipant(string chatId, string userId);
        Task RemoveParticipant(string chatId, string userId);
        Task<List<ChatParticipant>> GetChatParticipants(string chatId);
    }
}
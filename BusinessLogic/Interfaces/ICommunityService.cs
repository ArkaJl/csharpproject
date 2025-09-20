using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface ICommunityService
    {
        Task<List<Community>> GetAll();
        Task<Community?> GetById(string id);
        Task<List<Community>> GetByUserId(string userId);
        Task<Community> Create(Community community);
        Task<Community> Update(Community community);
        Task Delete(string id);
        Task SubscribeUser(string communityId, string userId, string role = "member");
        Task UnsubscribeUser(string communityId, string userId);
        Task<List<Subscription>> GetCommunitySubscribers(string communityId);
        Task<List<Subscription>> GetUserSubscriptions(string userId);
        Task UpdateMemberRole(string communityId, string userId, string role);
    }
}
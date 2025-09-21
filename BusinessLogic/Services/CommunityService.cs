
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CommunityService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Community>> GetAll()
        {
            return await _repositoryWrapper.Community.FindAll()
                .Include(c => c.Creator)
                .Include(c => c.Subscriptions)
                .Include(c => c.Posts)
                .Include(c => c.Albums)
                .Include(c => c.Chats)
                .ToListAsync();
        }

        public async Task<Community?> GetById(string id)
        {
            return await _repositoryWrapper.Community
                .FindByCondition(c => c.Id.ToString() == id)
                .Include(c => c.Creator)
                .Include(c => c.Subscriptions)
                .ThenInclude(s => s.User)
                .Include(c => c.Posts)
                .ThenInclude(p => p.Author)
                .Include(c => c.Albums)
                .Include(c => c.Chats)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Community>> GetByUserId(string userId)
        {
            return await _repositoryWrapper.Community
                .FindByCondition(c => c.Subscriptions.Any(s => s.UserId.ToString() == userId))
                .Include(c => c.Creator)
                .Include(c => c.Subscriptions)
                .ToListAsync();
        }

        public async Task<Community> Create(Community community)
        {
            await _repositoryWrapper.Community.Create(community);
            await _repositoryWrapper.SaveAsync();
            return community;
        }

        public async Task<Community> Update(Community community)
        {
            await _repositoryWrapper.Community.Update(community);
            await _repositoryWrapper.SaveAsync();
            return community;
        }

        public async Task Delete(string id)
        {
            var community = await _repositoryWrapper.Community
                .FindByCondition(c => c.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (community != null)
            {
                await _repositoryWrapper.Community.Delete(community);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task SubscribeUser(string communityId, string userId, string role = "member")
        {
            var subscription = new Subscription
            {
                CommunityId = Guid.Parse(communityId),
                UserId = Guid.Parse(userId),
                Role = role,
                JoinedAt = DateTime.UtcNow
            };

            await _repositoryWrapper.Subscription.Create(subscription);

            // Увеличиваем счетчик участников
            var community = await _repositoryWrapper.Community
                .FindByCondition(c => c.Id.ToString() == communityId)
                .FirstOrDefaultAsync();

            if (community != null)
            {
                community.MemberCount++;
                await _repositoryWrapper.Community.Update(community);
            }

            await _repositoryWrapper.SaveAsync();
        }

        public async Task UnsubscribeUser(string communityId, string userId)
        {
            var subscription = await _repositoryWrapper.Subscription
                .FindByCondition(s => s.CommunityId.ToString() == communityId && s.UserId.ToString() == userId)
                .FirstOrDefaultAsync();

            if (subscription != null)
            {
                await _repositoryWrapper.Subscription.Delete(subscription);

                // Уменьшаем счетчик участников
                var community = await _repositoryWrapper.Community
                    .FindByCondition(c => c.Id.ToString() == communityId)
                    .FirstOrDefaultAsync();

                if (community != null && community.MemberCount > 0)
                {
                    community.MemberCount--;
                    await _repositoryWrapper.Community.Update(community);
                }

                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task<List<Subscription>> GetCommunitySubscribers(string communityId)
        {
            return await _repositoryWrapper.Subscription
                .FindByCondition(s => s.CommunityId.ToString() == communityId)
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<List<Subscription>> GetUserSubscriptions(string userId)
        {
            return await _repositoryWrapper.Subscription
                .FindByCondition(s => s.UserId.ToString() == userId)
                .Include(s => s.Community)
                .ToListAsync();
        }

        public async Task UpdateMemberRole(string communityId, string userId, string role)
        {
            var subscription = await _repositoryWrapper.Subscription
                .FindByCondition(s => s.CommunityId.ToString() == communityId && s.UserId.ToString() == userId)
                .FirstOrDefaultAsync();

            if (subscription != null)
            {
                subscription.Role = role;
                await _repositoryWrapper.Subscription.Update(subscription);
                await _repositoryWrapper.SaveAsync();
            }
        }
    }
}
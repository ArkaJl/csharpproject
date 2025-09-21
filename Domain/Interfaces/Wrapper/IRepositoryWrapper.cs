using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Wrapper
{
    public interface IRepositoryWrapper
    {
        IUserRepository User {  get; }
        IUserInventoryRepository UserInventory { get; }
        IAlbumRepository Album { get; }
        IChatRepository Chat { get; }
        IChatParticipantRepository ChatParticipant { get; }
        ICommunityRepository Community { get; }
        ISubscriptionRepository Subscription { get; }
        IPostRepository Post { get; }
        ICommentRepository Comment { get; }
        INotificationRepository Notification { get; }
        IMessageRepository Message { get; }
        IStoreItemRepository StoreItem { get; }
        ITransactionRepository Transaction { get; }
        IMediaRepository Media { get; }
        void Save();
        Task SaveAsync();
    }
}

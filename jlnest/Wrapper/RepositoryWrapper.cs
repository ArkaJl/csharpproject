using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using System;

namespace DataAccess.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly JlnestContext _repoContext;
        private IUserRepository? _user;
        private IUserInventoryRepository? _userInventory;
        private IAlbumRepository? _albumInventory;
        private IChatRepository? _chat;
        private IChatParticipantRepository? _chatParticipant;
        private ICommunityRepository? _community;
        private ISubscriptionRepository? _subscription;
        private IPostRepository? _post; 
        private ICommentRepository? _comment;
        private INotificationRepository? _notification;
        private IMessageRepository? _message;
        private IStoreItemRepository? _storeItem;
        private ITransactionRepository? _transaction;
        private IMediaRepository? _media;

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        public IUserInventoryRepository UserInventory
        {
            get
            {
                if (_userInventory == null)
                {
                    _userInventory = new UserInventoryRepository(_repoContext);
                }
                return _userInventory;
            }
        }

        public IAlbumRepository Album
        {
            get
            {
                if (_albumInventory == null)
                {
                    _albumInventory = new AlbumRepository(_repoContext);
                }
                return _albumInventory;
            }
        }

        public IChatRepository Chat
        {
            get
            {
                if (_chat == null)
                {
                    _chat = new ChatRepository(_repoContext);
                }
                return _chat;
            }
        }

        public IChatParticipantRepository ChatParticipant
        {
            get
            {
                if (_chatParticipant == null)
                {
                    _chatParticipant = new ChatParticipantRepository(_repoContext);
                }
                return _chatParticipant;
            }
        }

        public ICommunityRepository Community
        {
            get
            {
                if (_community == null)
                {
                    _community = new CommunityRepository(_repoContext);
                }
                return _community;
            }
        }

        public ISubscriptionRepository Subscription
        {
            get
            {
                if (_subscription == null)
                {
                    _subscription = new SubscriptionRepository(_repoContext);
                }
                return _subscription;
            }
        }

        public ICommentRepository Comment
        {
            get
            {
                if (_comment == null)
                {
                    _comment = new CommentRepository(_repoContext);
                }
                return _comment;
            }
        }

        public IPostRepository Post
        {
            get
            {
                if (_post == null)
                {
                    _post = new PostRepository(_repoContext);
                }
                return _post;
            }
        }

        public INotificationRepository Notification
        {
            get
            {
                if (_notification == null)
                {
                    _notification = new NotificationRepository(_repoContext);
                }
                return _notification;
            }
        }

        public IMessageRepository Message
        {
            get
            {
                if (_message == null)
                {
                    _message = new MessageRepository(_repoContext);
                }
                return _message;
            }
        }

        public IStoreItemRepository StoreItem
        {
            get
            {
                if (_storeItem == null)
                {
                    _storeItem = new StoreItemRepository(_repoContext);
                }
                return _storeItem;
            }
        }

        public ITransactionRepository Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new TransactionRepository(_repoContext);
                }
                return _transaction;
            }
        }

        public IMediaRepository Media
        {
            get
            {
                if (_media == null)
                {
                    _media = new MediaRepository(_repoContext);
                }
                return _media;
            }
        }

        public RepositoryWrapper(JlnestContext repositoryContext)
        {
            _repoContext = repositoryContext ?? throw new ArgumentNullException(nameof(repositoryContext));
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
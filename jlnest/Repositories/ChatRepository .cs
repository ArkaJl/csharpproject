using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;


namespace DataAccess.Repositories
{
    public class ChatRepository : RepositoryBase<Chat>, IChatRepository
    {
        public ChatRepository(JlnestContext repositoryContext)
    : base(repositoryContext)
        {

        }
    }
}


using Domain.Interfaces.Repositories;
using Domain.Models;


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

using DataAccess.Interfaces;
using DataAccess.Models;


namespace DataAccess.Repositories
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;

namespace DataAccess.Repositories
{
    public class ChatParticipantRepository : RepositoryBase<ChatParticipant>, IChatParticipantRepository
    {
        public ChatParticipantRepository(JlnestContext repositoryContext)
            : base(repositoryContext)
        {

        }
    }
}

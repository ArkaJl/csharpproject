
using Domain.Interfaces.Repositories;
using Domain.Models;

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

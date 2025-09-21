
using Domain.Interfaces.Repositories;
using Domain.Models;


namespace DataAccess.Repositories
{
    public class CommunityRepository : RepositoryBase<Community>, ICommunityRepository
    {
        public CommunityRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

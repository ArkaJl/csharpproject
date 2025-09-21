
using Domain.Interfaces.Repositories;
using Domain.Models;


namespace DataAccess.Repositories
{
    public class CommunityOverviewRepository : RepositoryBase<CommunityOverview>, ICommunityOverviewRepository
    {
        public CommunityOverviewRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

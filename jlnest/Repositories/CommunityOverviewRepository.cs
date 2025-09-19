using DataAccess.Interfaces;
using DataAccess.Models;


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

using DataAccess.Interfaces;
using DataAccess.Models;


namespace DataAccess.Repositories
{
    public class SubscriptionRepository : RepositoryBase<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

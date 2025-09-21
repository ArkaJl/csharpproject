
using Domain.Interfaces.Repositories;
using Domain.Models;


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

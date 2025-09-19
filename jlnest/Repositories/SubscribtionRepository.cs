using DataAccess.Interfaces;
using DataAccess.Models;


namespace DataAccess.Repositories
{
    public class SubscribtionRepository : RepositoryBase<Subscription>, ISubscribtionRepository
    {
        public SubscribtionRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

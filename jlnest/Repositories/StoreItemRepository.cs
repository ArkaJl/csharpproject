
using Domain.Interfaces.Repositories;
using Domain.Models;


namespace DataAccess.Repositories
{
    public class StoreItemRepository : RepositoryBase<StoreItem>, IStoreItemRepository
    {
        public StoreItemRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

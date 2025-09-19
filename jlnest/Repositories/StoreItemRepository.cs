using DataAccess.Interfaces;
using DataAccess.Models;


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

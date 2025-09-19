using DataAccess.Interfaces;
using DataAccess.Models;


namespace DataAccess.Repositories
{
    public class UserInventoryRepository : RepositoryBase<UserInventory>, IUserInventoryRepository
    {
        public UserInventoryRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

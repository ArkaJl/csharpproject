    
using Domain.Interfaces.Repositories;
using Domain.Models;


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

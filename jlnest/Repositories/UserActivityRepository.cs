
using Domain.Interfaces.Repositories;
using Domain.Models;


namespace DataAccess.Repositories
{
    public class UserActivityRepository : RepositoryBase<UserActivity>, IUserActivityRepository
    {
        public UserActivityRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

using DataAccess.Interfaces;
using DataAccess.Models;


namespace DataAccess.Repositories
{
    public class MediumRepository : RepositoryBase<Medium>, IMediumRepository
    {
        public MediumRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

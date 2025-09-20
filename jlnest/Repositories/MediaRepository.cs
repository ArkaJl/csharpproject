using DataAccess.Interfaces;
using DataAccess.Models;


namespace DataAccess.Repositories
{
    public class MediaRepository : RepositoryBase<Medium>, IMediaRepository
    {
        public MediaRepository(JlnestContext repositoryContext)
: base(repositoryContext)
        {

        }
    }
}

using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;


namespace DataAccess.Repositories
{
    public class AlbumRepository : RepositoryBase<Album>, IAlbumRepository
    {
        public AlbumRepository(JlnestContext repositoryContext)
    : base(repositoryContext)
        {

        }
    }
}

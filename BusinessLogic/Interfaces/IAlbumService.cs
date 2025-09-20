using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface IAlbumService
    {
        Task<List<Album>> GetAll();
        Task<Album?> GetById(string id);
        Task<List<Album>> GetByCommunityId(string communityId);
        Task<Album> Create(Album album);
        Task<Album> Update(Album album);
        Task Delete(string id);
    }
}
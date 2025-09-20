using DataAccess.Models;

namespace BusinessLogic.Interfaces
{
    public interface IMediaService
    {
        Task<List<Medium>> GetAll();
        Task<Medium?> GetById(string id);
        Task<List<Medium>> GetByAlbumId(string albumId);
        Task<List<Medium>> GetByUserId(string userId);
        Task<List<Medium>> GetByType(string type);
        Task<Medium> Create(Medium medium);
        Task<Medium> Update(Medium medium);
        Task Delete(string id);
        Task<List<Medium>> GetRecentMedia(int count);
    }
}
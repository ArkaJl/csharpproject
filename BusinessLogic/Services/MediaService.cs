using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class MediaService : IMediaService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public MediaService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Medium>> GetAll()
        {
            return await _repositoryWrapper.Media.FindAll()
                .Include(m => m.Album)
                .Include(m => m.UploadedByNavigation)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Medium?> GetById(string id)
        {
            return await _repositoryWrapper.Media
                .FindByCondition(m => m.Id.ToString() == id)
                .Include(m => m.Album)
                .Include(m => m.UploadedByNavigation)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Medium>> GetByAlbumId(string albumId)
        {
            return await _repositoryWrapper.Media
                .FindByCondition(m => m.AlbumId.ToString() == albumId)
                .Include(m => m.Album)
                .Include(m => m.UploadedByNavigation)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Medium>> GetByUserId(string userId)
        {
            return await _repositoryWrapper.Media
                .FindByCondition(m => m.UploadedBy.ToString() == userId)
                .Include(m => m.Album)
                .Include(m => m.UploadedByNavigation)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Medium>> GetByType(string type)
        {
            return await _repositoryWrapper.Media
                .FindByCondition(m => m.Type == type)
                .Include(m => m.Album)
                .Include(m => m.UploadedByNavigation)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Medium> Create(Medium medium)
        {
            _repositoryWrapper.Media.Create(medium);
            await _repositoryWrapper.SaveAsync();
            return medium;
        }

        public async Task<Medium> Update(Medium medium)
        {
            _repositoryWrapper.Media.Update(medium);
            await _repositoryWrapper.SaveAsync();
            return medium;
        }

        public async Task Delete(string id)
        {
            var medium = await _repositoryWrapper.Media
                .FindByCondition(m => m.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (medium != null)
            {
                _repositoryWrapper.Media.Delete(medium);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task<List<Medium>> GetRecentMedia(int count)
        {
            return await _repositoryWrapper.Media.FindAll()
                .Include(m => m.Album)
                .Include(m => m.UploadedByNavigation)
                .OrderByDescending(m => m.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
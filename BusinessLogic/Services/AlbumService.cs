
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AlbumService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Album>> GetAll()
        {
            return await _repositoryWrapper.Album.FindAll()
                .Include(a => a.Community)
                .Include(a => a.Media)
                .ToListAsync();
        }

        public async Task<Album?> GetById(string id)
        {
            return await _repositoryWrapper.Album
                .FindByCondition(a => a.Id.ToString() == id)
                .Include(a => a.Community)
                .Include(a => a.Media)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Album>> GetByCommunityId(string communityId)
        {
            return await _repositoryWrapper.Album
                .FindByCondition(a => a.CommunityId.ToString() == communityId)
                .Include(a => a.Community)
                .Include(a => a.Media)
                .ToListAsync();
        }

        public async Task<Album> Create(Album album)
        {
            await _repositoryWrapper.Album.Create(album);
            await _repositoryWrapper.SaveAsync();
            return album;
        }

        public async Task<Album> Update(Album album)
        {
            await _repositoryWrapper.Album.Update(album);
            await _repositoryWrapper.SaveAsync();
            return album;
        }

        public async Task Delete(string id)
        {
            var album = await _repositoryWrapper.Album
                .FindByCondition(a => a.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (album != null)
            {
                await _repositoryWrapper.Album.Delete(album);
                await _repositoryWrapper.SaveAsync();
            }
        }
    }
}
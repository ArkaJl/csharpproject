using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Album;
using WebApplication1.Contracts.Media;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Контроллер для управления альбомами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        /// <summary>
        /// получить все альбомы
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var albums = await _albumService.GetAll();
            var response = albums.Select(a => new AlbumResponse
            {
                Id = a.Id,
                CommunityId = a.CommunityId,
                Name = a.Name,
                Description = a.Description,
                CreatedAt = a.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить альбом по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var album = await _albumService.GetById(id.ToString());
            if (album == null)
            {
                return NotFound($"Album with id {id} not found");
            }

            var response = new AlbumResponse
            {
                Id = album.Id,
                CommunityId = album.CommunityId,
                Name = album.Name,
                Description = album.Description,
                CreatedAt = album.CreatedAt
            };
            return Ok(response);
        }

        /// <summary>
        /// получить альбомы сообщества по id
        /// </summary>
        [HttpGet("community/{communityId}")]
        public async Task<IActionResult> GetByCommunityId(Guid communityId)
        {
            var albums = await _albumService.GetByCommunityId(communityId.ToString());
            var response = albums.Select(a => new AlbumResponse
            {
                Id = a.Id,
                CommunityId = a.CommunityId,
                Name = a.Name,
                Description = a.Description,
                CreatedAt = a.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// Создать альбом
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAlbumRequest request)
        {
            var album = new Album
            {
                CommunityId = request.CommunityId,
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            var createdAlbum = await _albumService.Create(album);

            var response = new AlbumResponse
            {
                Id = createdAlbum.Id,
                CommunityId = createdAlbum.CommunityId,
                Name = createdAlbum.Name,
                Description = createdAlbum.Description,
                CreatedAt = createdAlbum.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// изменить данные альбома
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateAlbumRequest request)
        {
            var existingAlbum = await _albumService.GetById(id.ToString());
            if (existingAlbum == null)
            {
                return NotFound($"Album with id {id} not found");
            }

            // Обновляем только переданные поля
            if (request.CommunityId != Guid.Empty)
                existingAlbum.CommunityId = request.CommunityId;

            if (!string.IsNullOrEmpty(request.Name))
                existingAlbum.Name = request.Name;

            if (request.Description != null)
                existingAlbum.Description = request.Description;

            var updatedAlbum = await _albumService.Update(existingAlbum);

            var response = new AlbumResponse
            {
                Id = updatedAlbum.Id,
                CommunityId = updatedAlbum.CommunityId,
                Name = updatedAlbum.Name,
                Description = updatedAlbum.Description,
                CreatedAt = updatedAlbum.CreatedAt
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить альбом
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingAlbum = await _albumService.GetById(id.ToString());
            if (existingAlbum == null)
            {
                return NotFound($"Album with id {id} not found");
            }

            await _albumService.Delete(id.ToString());
            return NoContent();
        }
    }
}
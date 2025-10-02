using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Media;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Контроллер для управления медиа
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        /// <summary>
        /// получить все медиа
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var media = await _mediaService.GetAll();
            var response = media.Select(m => new MediaResponse
            {
                Id = m.Id,
                AlbumId = m.AlbumId,
                Url = m.Url,
                Type = m.Type,
                UploadedBy = m.UploadedBy,
                CreatedAt = m.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить медиа по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var medium = await _mediaService.GetById(id.ToString());
            if (medium == null)
            {
                return NotFound($"Media with id {id} not found");
            }

            var response = new MediaResponse
            {
                Id = medium.Id,
                AlbumId = medium.AlbumId,
                Url = medium.Url,
                Type = medium.Type,
                UploadedBy = medium.UploadedBy,
                CreatedAt = medium.CreatedAt
            };
            return Ok(response);
        }

        /// <summary>
        /// получить медиа из альбома
        /// </summary>
        [HttpGet("album/{albumId}")]
        public async Task<IActionResult> GetByAlbumId(Guid albumId)
        {
            var media = await _mediaService.GetByAlbumId(albumId.ToString());
            var response = media.Select(m => new MediaResponse
            {
                Id = m.Id,
                AlbumId = m.AlbumId,
                Url = m.Url,
                Type = m.Type,
                UploadedBy = m.UploadedBy,
                CreatedAt = m.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить медиа пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var media = await _mediaService.GetByUserId(userId.ToString());
            var response = media.Select(m => new MediaResponse
            {
                Id = m.Id,
                AlbumId = m.AlbumId,
                Url = m.Url,
                Type = m.Type,
                UploadedBy = m.UploadedBy,
                CreatedAt = m.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить медиа по типу
        /// </summary>
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            // Валидация типа
            if (type.ToLower() != "image" && type.ToLower() != "video")
            {
                return BadRequest("Type must be either 'image' or 'video'");
            }

            var media = await _mediaService.GetByType(type);
            var response = media.Select(m => new MediaResponse
            {
                Id = m.Id,
                AlbumId = m.AlbumId,
                Url = m.Url,
                Type = m.Type,
                UploadedBy = m.UploadedBy,
                CreatedAt = m.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// получить последние несколько медиа
        /// </summary>
        [HttpGet("recent/{count}")]
        public async Task<IActionResult> GetRecentMedia(int count)
        {
            if (count <= 0 || count > 100)
            {
                return BadRequest("Count must be between 1 and 100");
            }

            var media = await _mediaService.GetRecentMedia(count);
            var response = media.Select(m => new MediaResponse
            {
                Id = m.Id,
                AlbumId = m.AlbumId,
                Url = m.Url,
                Type = m.Type,
                UploadedBy = m.UploadedBy,
                CreatedAt = m.CreatedAt
            });
            return Ok(response);
        }

        /// <summary>
        /// создать медиа
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UploadMediaRequest request)
        {
            // Валидация типа
            if (request.Type.ToLower() != "image" && request.Type.ToLower() != "video")
            {
                return BadRequest("Type must be either 'image' or 'video'");
            }

            var medium = new Medium
            {
                AlbumId = request.AlbumId,
                Url = request.Url,
                Type = request.Type.ToLower(), // Приводим к нижнему регистру для консистентности
                UploadedBy = request.UploadedBy,
                CreatedAt = DateTime.UtcNow
            };

            var createdMedia = await _mediaService.Create(medium);

            var response = new MediaResponse
            {
                Id = createdMedia.Id,
                AlbumId = createdMedia.AlbumId,
                Url = createdMedia.Url,
                Type = createdMedia.Type,
                UploadedBy = createdMedia.UploadedBy,
                CreatedAt = createdMedia.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// изменить медиа
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UploadMediaRequest request)
        {
            var existingMedia = await _mediaService.GetById(id.ToString());
            if (existingMedia == null)
            {
                return NotFound($"Media with id {id} not found");
            }

            // Валидация типа если передан
            if (!string.IsNullOrEmpty(request.Type) &&
                request.Type.ToLower() != "image" && request.Type.ToLower() != "video")
            {
                return BadRequest("Type must be either 'image' or 'video'");
            }

            // Обновляем только переданные поля
            if (request.AlbumId != Guid.Empty)
                existingMedia.AlbumId = request.AlbumId;

            if (!string.IsNullOrEmpty(request.Url))
                existingMedia.Url = request.Url;

            if (!string.IsNullOrEmpty(request.Type))
                existingMedia.Type = request.Type.ToLower();

            if (request.UploadedBy != Guid.Empty)
                existingMedia.UploadedBy = request.UploadedBy;

            var updatedMedia = await _mediaService.Update(existingMedia);

            var response = new MediaResponse
            {
                Id = updatedMedia.Id,
                AlbumId = updatedMedia.AlbumId,
                Url = updatedMedia.Url,
                Type = updatedMedia.Type,
                UploadedBy = updatedMedia.UploadedBy,
                CreatedAt = updatedMedia.CreatedAt
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить медиа по id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingMedia = await _mediaService.GetById(id.ToString());
            if (existingMedia == null)
            {
                return NotFound($"Media with id {id} not found");
            }

            await _mediaService.Delete(id.ToString());
            return NoContent();
        }
    }
}
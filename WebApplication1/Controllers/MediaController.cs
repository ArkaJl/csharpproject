using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(media);
        }
        /// <summary>
        /// получить медиа по id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var medium = await _mediaService.GetById(id);
            if (medium == null)
            {
                return NotFound($"Media with id {id} not found");
            }
            return Ok(medium);
        }
        /// <summary>
        /// получить медиа из альбома
        /// </summary>

        [HttpGet("album/{albumId}")]
        public async Task<IActionResult> GetByAlbumId(string albumId)
        {
            var media = await _mediaService.GetByAlbumId(albumId);
            return Ok(media);
        }
        /// <summary>
        /// получить медиа пользователя
        /// </summary>

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var media = await _mediaService.GetByUserId(userId);
            return Ok(media);
        }
        /// <summary>
        /// получить медиа типа
        /// </summary>

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var media = await _mediaService.GetByType(type);
            return Ok(media);
        }
        /// <summary>
        /// получить последние несколько медиа
        /// </summary>

        [HttpGet("recent/{count}")]
        public async Task<IActionResult> GetRecentMedia(int count)
        {
            var media = await _mediaService.GetRecentMedia(count);
            return Ok(media);
        }
        /// <summary>
        /// создать медиа
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Medium medium)
        {
            var createdMedia = await _mediaService.Create(medium);
            return CreatedAtAction(nameof(GetById), new { id = createdMedia.Id }, createdMedia);
        }
        /// <summary>
        /// изменить медиа
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Medium medium)
        {
            if (id != medium.Id.ToString())
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedMedia = await _mediaService.Update(medium);
            return Ok(updatedMedia);
        }
        /// <summary>
        /// удалить медиа по id
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediaService.Delete(id);
            return NoContent();
        }
    }
}
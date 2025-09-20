using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var media = await _mediaService.GetAll();
            return Ok(media);
        }

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

        [HttpGet("album/{albumId}")]
        public async Task<IActionResult> GetByAlbumId(string albumId)
        {
            var media = await _mediaService.GetByAlbumId(albumId);
            return Ok(media);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var media = await _mediaService.GetByUserId(userId);
            return Ok(media);
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var media = await _mediaService.GetByType(type);
            return Ok(media);
        }

        [HttpGet("recent/{count}")]
        public async Task<IActionResult> GetRecentMedia(int count)
        {
            var media = await _mediaService.GetRecentMedia(count);
            return Ok(media);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Medium medium)
        {
            var createdMedia = await _mediaService.Create(medium);
            return CreatedAtAction(nameof(GetById), new { id = createdMedia.Id }, createdMedia);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediaService.Delete(id);
            return NoContent();
        }
    }
}
using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(albums);
        }
        /// <summary>
        /// получить альбомы по id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var album = await _albumService.GetById(id);
            if (album == null)
            {
                return NotFound($"Album with id {id} not found");
            }
            return Ok(album);
        }
        /// <summary>
        /// получить альбомы сообщества по id
        /// </summary>

        [HttpGet("community/{communityId}")]
        public async Task<IActionResult> GetByCommunityId(string communityId)
        {
            var albums = await _albumService.GetByCommunityId(communityId);
            return Ok(albums);
        }
        /// <summary>
        /// Создать альбом
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Album album)
        {
            var createdAlbum = await _albumService.Create(album);
            return CreatedAtAction(nameof(GetById), new { id = createdAlbum.Id }, createdAlbum);
        }
        /// <summary>
        /// изменить данные альбома
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Album album)
        {
            if (id != album.Id.ToString())
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedAlbum = await _albumService.Update(album);
            return Ok(updatedAlbum);
        }
        /// <summary>
        /// удалить альбом
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _albumService.Delete(id);
            return NoContent();
        }
    }
}
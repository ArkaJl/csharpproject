using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var albums = await _albumService.GetAll();
            return Ok(albums);
        }

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

        [HttpGet("community/{communityId}")]
        public async Task<IActionResult> GetByCommunityId(string communityId)
        {
            var albums = await _albumService.GetByCommunityId(communityId);
            return Ok(albums);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Album album)
        {
            var createdAlbum = await _albumService.Create(album);
            return CreatedAtAction(nameof(GetById), new { id = createdAlbum.Id }, createdAlbum);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _albumService.Delete(id);
            return NoContent();
        }
    }
}
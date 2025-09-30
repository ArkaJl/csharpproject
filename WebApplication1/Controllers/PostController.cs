using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    /// <summary>
    /// Контроллер для управления постами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        /// <summary>
        /// получить все посты
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetAll();
            return Ok(posts);
        }
        /// <summary>
        /// получить пост по id
        /// </summary>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var post = await _postService.GetById(id);
            if (post == null)
            {
                return NotFound($"Post with id {id} not found");
            }
            return Ok(post);
        }
        /// <summary>
        /// получить посты пользователя
        /// </summary>

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var posts = await _postService.GetByUserId(userId);
            return Ok(posts);
        }
        /// <summary>
        /// получить посты в сообществе
        /// </summary>

        [HttpGet("community/{communityId}")]
        public async Task<IActionResult> GetByCommunityId(string communityId)
        {
            var posts = await _postService.GetByCommunityId(communityId);
            return Ok(posts);
        }
        /// <summary>
        /// создать пост
        /// </summary>

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            var createdPost = await _postService.Create(post);
            return CreatedAtAction(nameof(GetById), new { id = createdPost.Id }, createdPost);
        }
        /// <summary>
        /// изменить пост
        /// </summary>

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Post post)
        {
            if (id != post.Id.ToString())
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            var updatedPost = await _postService.Update(post);
            return Ok(updatedPost);
        }
        /// <summary>
        /// удалить пост
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _postService.Delete(id);
            return NoContent();
        }
        /// <summary>
        /// пометить нравится пользователем пост
        /// </summary>

        [HttpPost("{postId}/like/{userId}")]
        public async Task<IActionResult> LikePost(string postId, string userId)
        {
            var likesCount = await _postService.LikePost(postId, userId);
            return Ok(new { LikesCount = likesCount });
        }
        /// <summary>
        /// пометить ненравится пользователем пост
        /// </summary>

        [HttpPost("{postId}/unlike/{userId}")]
        public async Task<IActionResult> UnlikePost(string postId, string userId)
        {
            var likesCount = await _postService.UnlikePost(postId, userId);
            return Ok(new { LikesCount = likesCount });
        }
        /// <summary>
        /// добавить комментарий посту
        /// </summary>

        [HttpPost("{postId}/comments")]
        public async Task<IActionResult> AddComment(string postId, [FromBody] Comment comment)
        {
            var commentsCount = await _postService.AddComment(postId, comment);
            return Ok(new { CommentsCount = commentsCount });
        }
        /// <summary>
        /// получить комментарии поста
        /// </summary>

        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetPostComments(string postId)
        {
            var comments = await _postService.GetPostComments(postId);
            return Ok(comments);
        }
    }
}
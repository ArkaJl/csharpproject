using Domain.Models;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Post;
using WebApplication1.Contracts.Comment;

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
            var response = posts.Select(p => new PostResponse
            {
                Id = p.Id,
                AuthorId = p.AuthorId,
                CommunityId = p.CommunityId,
                Content = p.Content,
                Images = p.Images,
                LikesCount = p.LikesCount,
                CommentsCount = p.CommentsCount,
                CreatedAt = p.CreatedAt,
                Visibility = p.Visibility
            });
            return Ok(response);
        }

        /// <summary>
        /// получить пост по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var post = await _postService.GetById(id.ToString());
            if (post == null)
            {
                return NotFound($"Post with id {id} not found");
            }

            var response = new PostResponse
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                CommunityId = post.CommunityId,
                Content = post.Content,
                Images = post.Images,
                LikesCount = post.LikesCount,
                CommentsCount = post.CommentsCount,
                CreatedAt = post.CreatedAt,
                Visibility = post.Visibility
            };
            return Ok(response);
        }

        /// <summary>
        /// получить посты пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var posts = await _postService.GetByUserId(userId.ToString());
            var response = posts.Select(p => new PostResponse
            {
                Id = p.Id,
                AuthorId = p.AuthorId,
                CommunityId = p.CommunityId,
                Content = p.Content,
                Images = p.Images,
                LikesCount = p.LikesCount,
                CommentsCount = p.CommentsCount,
                CreatedAt = p.CreatedAt,
                Visibility = p.Visibility
            });
            return Ok(response);
        }

        /// <summary>
        /// получить посты в сообществе
        /// </summary>
        [HttpGet("community/{communityId}")]
        public async Task<IActionResult> GetByCommunityId(Guid communityId)
        {
            var posts = await _postService.GetByCommunityId(communityId.ToString());
            var response = posts.Select(p => new PostResponse
            {
                Id = p.Id,
                AuthorId = p.AuthorId,
                CommunityId = p.CommunityId,
                Content = p.Content,
                Images = p.Images,
                LikesCount = p.LikesCount,
                CommentsCount = p.CommentsCount,
                CreatedAt = p.CreatedAt,
                Visibility = p.Visibility
            });
            return Ok(response);
        }

        /// <summary>
        /// создать пост
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
        {
            // Валидация видимости поста
            var validVisibility = new[] { "public", "private", "subscribers" };
            if (!validVisibility.Contains(request.Visibility?.ToLower()))
            {
                return BadRequest($"Visibility must be one of: {string.Join(", ", validVisibility)}");
            }

            var post = new Post
            {
                AuthorId = request.AuthorId,
                CommunityId = request.CommunityId,
                Content = request.Content,
                Images = request.Images,
                LikesCount = 0,
                CommentsCount = 0,
                Visibility = request.Visibility ?? "public",
                CreatedAt = DateTime.UtcNow
            };

            var createdPost = await _postService.Create(post);

            var response = new PostResponse
            {
                Id = createdPost.Id,
                AuthorId = createdPost.AuthorId,
                CommunityId = createdPost.CommunityId,
                Content = createdPost.Content,
                Images = createdPost.Images,
                LikesCount = createdPost.LikesCount,
                CommentsCount = createdPost.CommentsCount,
                CreatedAt = createdPost.CreatedAt,
                Visibility = createdPost.Visibility
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// изменить пост
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostRequest request)
        {
            var existingPost = await _postService.GetById(id.ToString());
            if (existingPost == null)
            {
                return NotFound($"Post with id {id} not found");
            }

            // Валидация видимости поста если передана
            if (!string.IsNullOrEmpty(request.Visibility))
            {
                var validVisibility = new[] { "public", "private", "subscribers" };
                if (!validVisibility.Contains(request.Visibility.ToLower()))
                {
                    return BadRequest($"Visibility must be one of: {string.Join(", ", validVisibility)}");
                }
                existingPost.Visibility = request.Visibility;
            }

            // Обновляем только переданные поля
            if (!string.IsNullOrEmpty(request.Content))
                existingPost.Content = request.Content;

            if (request.Images != null)
                existingPost.Images = request.Images;

            var updatedPost = await _postService.Update(existingPost);

            var response = new PostResponse
            {
                Id = updatedPost.Id,
                AuthorId = updatedPost.AuthorId,
                CommunityId = updatedPost.CommunityId,
                Content = updatedPost.Content,
                Images = updatedPost.Images,
                LikesCount = updatedPost.LikesCount,
                CommentsCount = updatedPost.CommentsCount,
                CreatedAt = updatedPost.CreatedAt,
                Visibility = updatedPost.Visibility
            };

            return Ok(response);
        }

        /// <summary>
        /// удалить пост
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingPost = await _postService.GetById(id.ToString());
            if (existingPost == null)
            {
                return NotFound($"Post with id {id} not found");
            }

            await _postService.Delete(id.ToString());
            return NoContent();
        }

        /// <summary>
        /// пометить нравится пользователем пост
        /// </summary>
        [HttpPost("{postId}/like/{userId}")]
        public async Task<IActionResult> LikePost(Guid postId, Guid userId)
        {
            var likesCount = await _postService.LikePost(postId.ToString(), userId.ToString());
            return Ok(new { LikesCount = likesCount });
        }

        /// <summary>
        /// пометить ненравится пользователем пост
        /// </summary>
        [HttpPost("{postId}/unlike/{userId}")]
        public async Task<IActionResult> UnlikePost(Guid postId, Guid userId)
        {
            var likesCount = await _postService.UnlikePost(postId.ToString(), userId.ToString());
            return Ok(new { LikesCount = likesCount });
        }

        /// <summary>
        /// добавить комментарий посту
        /// </summary>
        [HttpPost("{postId}/comments")]
        public async Task<IActionResult> AddComment(Guid postId, [FromBody] CreateCommentRequest request)
        {
            var comment = new Comment
            {
                PostId = postId,
                AuthorId = request.AuthorId,
                Text = request.Text,
                LikesCount = 0,
                CreatedAt = DateTime.UtcNow
            };

            var commentsCount = await _postService.AddComment(postId.ToString(), comment);
            return Ok(new { CommentsCount = commentsCount });
        }

        /// <summary>
        /// получить комментарии поста
        /// </summary>
        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetPostComments(Guid postId)
        {
            var comments = await _postService.GetPostComments(postId.ToString());
            var response = comments.Select(c => new CommentResponse
            {
                Id = c.Id,
                PostId = c.PostId,
                AuthorId = c.AuthorId,
                Text = c.Text,
                LikesCount = c.LikesCount,
                CreatedAt = c.CreatedAt
            });
            return Ok(response);
        }
    }
}

using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class PostService : IPostService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public PostService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Post>> GetAll()
        {
            return await _repositoryWrapper.Post.FindAll()
                .Include(p => p.Author)
                .Include(p => p.Community)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post?> GetById(string id)
        {
            return await _repositoryWrapper.Post
                .FindByCondition(p => p.Id.ToString() == id)
                .Include(p => p.Author)
                .Include(p => p.Community)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Post>> GetByUserId(string userId)
        {
            return await _repositoryWrapper.Post
                .FindByCondition(p => p.AuthorId.ToString() == userId)
                .Include(p => p.Author)
                .Include(p => p.Community)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Post>> GetByCommunityId(string communityId)
        {
            return await _repositoryWrapper.Post
                .FindByCondition(p => p.CommunityId.ToString() == communityId)
                .Include(p => p.Author)
                .Include(p => p.Community)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post> Create(Post post)
        {
            await _repositoryWrapper.Post.Create(post);
            await _repositoryWrapper.SaveAsync();
            return post;
        }

        public async Task<Post> Update(Post post)
        {
            await _repositoryWrapper.Post.Update(post);
            await _repositoryWrapper.SaveAsync();
            return post;
        }

        public async Task Delete(string id)
        {
            var post = await _repositoryWrapper.Post
                .FindByCondition(p => p.Id.ToString() == id)
                .FirstOrDefaultAsync();

            if (post != null)
            {
                await _repositoryWrapper.Post.Delete(post);
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task<int> LikePost(string postId, string userId)
        {
            // Используем синхронный FirstOrDefault вместо FirstOrDefaultAsync
            var post = _repositoryWrapper.Post
                .FindByCondition(p => p.Id.ToString() == postId)
                .FirstOrDefault(); // убрали Async

            if (post != null)
            {
                post.LikesCount++;
                await _repositoryWrapper.Post.Update(post);
                await _repositoryWrapper.SaveAsync();
                return post.LikesCount;
            }

            return 0;
        }

        public async Task<int> UnlikePost(string postId, string userId)
        {
            // Используем синхронный FirstOrDefault вместо FirstOrDefaultAsync
            var post = _repositoryWrapper.Post
                .FindByCondition(p => p.Id.ToString() == postId)
                .FirstOrDefault(); // убрали Async

            if (post != null && post.LikesCount > 0)
            {
                post.LikesCount--;
                await _repositoryWrapper.Post.Update(post);
                await _repositoryWrapper.SaveAsync();
                return post.LikesCount;
            }

            return 0;
        }

        public async Task<int> AddComment(string postId, Comment comment)
        {
            // Используем синхронный FirstOrDefault вместо FirstOrDefaultAsync
            var post = _repositoryWrapper.Post
                .FindByCondition(p => p.Id.ToString() == postId)
                .FirstOrDefault(); // убрали Async

            if (post != null)
            {
                comment.PostId = Guid.Parse(postId);
                await _repositoryWrapper.Comment.Create(comment);

                post.CommentsCount++;
                await _repositoryWrapper.Post.Update(post);

                await _repositoryWrapper.SaveAsync();
                return post.CommentsCount;
            }

            return 0;
        }

        public async Task<List<Comment>> GetPostComments(string postId)
        {
            return await _repositoryWrapper.Comment
                .FindByCondition(c => c.PostId.ToString() == postId)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
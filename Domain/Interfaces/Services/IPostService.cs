using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAll();
        Task<Post?> GetById(string id);
        Task<List<Post>> GetByUserId(string userId);
        Task<List<Post>> GetByCommunityId(string communityId);
        Task<Post> Create(Post post);
        Task<Post> Update(Post post);
        Task Delete(string id);
        Task<int> LikePost(string postId, string userId);
        Task<int> UnlikePost(string postId, string userId);
        Task<int> AddComment(string postId, Comment comment);
        Task<List<Comment>> GetPostComments(string postId);
    }
}
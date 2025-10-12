using Moq;
using BusinessLogic.Services;
using Domain.Interfaces.Repositories;
using Domain.Wrapper;
using Domain.Models;

namespace BusinessLogic.Tests
{
    public class PostServiceTests
    {
        private readonly PostService _service;
        private readonly Mock<IPostRepository> _postRepositoryMoq;
        private readonly Mock<ICommentRepository> _commentRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public PostServiceTests()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _postRepositoryMoq = new Mock<IPostRepository>();
            _commentRepositoryMoq = new Mock<ICommentRepository>();

            _repositoryWrapperMoq.Setup(x => x.Post).Returns(_postRepositoryMoq.Object);
            _repositoryWrapperMoq.Setup(x => x.Comment).Returns(_commentRepositoryMoq.Object);

            _service = new PostService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidPost_ShouldCreatePost()
        {
            // Arrange
            var post = new Post
            {
                Content = "Test Content",
                AuthorId = Guid.NewGuid()
            };

            // Act
            var result = await _service.Create(post);

            // Assert
            _postRepositoryMoq.Verify(x => x.Create(post), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(post, result);
        }

        [Fact]
        public async Task UpdateAsync_ValidPost_ShouldUpdatePost()
        {
            // Arrange
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Content = "Updated Content"
            };

            // Act
            var result = await _service.Update(post);

            // Assert
            _postRepositoryMoq.Verify(x => x.Update(post), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(post, result);
        }

        [Fact]
        public async Task LikePostAsync_ValidPost_ShouldIncrementLikes()
        {
            // Arrange
            var postId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            var post = new Post { Id = Guid.Parse(postId), LikesCount = 10 };

            _postRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Post, bool>>>()))
                .Returns(new List<Post> { post }.AsQueryable());

            // Act
            var result = await _service.LikePost(postId, userId);

            // Assert
            _postRepositoryMoq.Verify(x => x.Update(It.Is<Post>(p => p.LikesCount == 11)), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(11, result);
        }

        [Fact]
        public async Task AddCommentAsync_ValidComment_ShouldAddCommentAndIncrementCount()
        {
            // Arrange
            var postId = Guid.NewGuid().ToString();
            var comment = new Comment
            {
                Text = "Test comment",
                AuthorId = Guid.NewGuid()
            };
            var post = new Post { Id = Guid.Parse(postId), CommentsCount = 5 };

            _postRepositoryMoq.Setup(x => x.FindByCondition(It.IsAny<System.Linq.Expressions.Expression<System.Func<Post, bool>>>()))
                .Returns(new List<Post> { post }.AsQueryable());

            // Act
            var result = await _service.AddComment(postId, comment);

            // Assert
            _commentRepositoryMoq.Verify(x => x.Create(It.Is<Comment>(c => c.PostId == Guid.Parse(postId))), Times.Once);
            _postRepositoryMoq.Verify(x => x.Update(It.Is<Post>(p => p.CommentsCount == 6)), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(6, result);
        }

        [Fact]
        public async Task AddCommentAsync_EmptyPostId_ShouldNotAddComment()
        {
            // Arrange
            var comment = new Comment { Text = "Test comment", AuthorId = Guid.NewGuid() };

            // Act
            var result = await _service.AddComment("", comment);

            // Assert
            _commentRepositoryMoq.Verify(x => x.Create(It.IsAny<Comment>()), Times.Never);
            Assert.Equal(0, result);
        }
    }
}
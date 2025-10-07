using Moq;
using BusinessLogic.Services;
using Domain.Interfaces.Repositories;
using Domain.Wrapper;
using Domain.Models;
using Xunit;

namespace BusinessLogic.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _service;
        private readonly Mock<IUserRepository> _userRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public UserServiceTests()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _userRepositoryMoq = new Mock<IUserRepository>();

            _repositoryWrapperMoq.Setup(x => x.User)
                .Returns(_userRepositoryMoq.Object);

            _service = new UserService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullUser_ShouldThrowNullArgumentException()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => _service.Create(null));

            Assert.IsType<ArgumentNullException>(ex);
            _userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Never);
        }
    }
}
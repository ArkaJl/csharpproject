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
            _repositoryWrapperMoq.Setup(x => x.User).Returns(_userRepositoryMoq.Object);
            _service = new UserService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidUser_ShouldCreateUser()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@test.com",
                PasswordHash = "hashedpassword"
            };

            // Act
            await _service.Create(user);

            // Assert
            _userRepositoryMoq.Verify(x => x.Create(user), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_NullUser_ShouldThrowException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.Create(null));
            _userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetInvalidUsersForCreate))]
        public async Task CreateAsync_UserWithInvalidData_ShouldThrowException(User user)
        {
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Create(user));
            _userRepositoryMoq.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ValidUser_ShouldUpdateUser()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "updateduser",
                Email = "updated@test.com",
                PasswordHash = "newpassword"
            };

            // Act
            await _service.Update(user);

            // Assert
            _userRepositoryMoq.Verify(x => x.Update(user), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullUser_ShouldThrowException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.Update(null));
            _userRepositoryMoq.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Never);
        }

        public static IEnumerable<object[]> GetInvalidUsersForCreate()
        {
            return new List<object[]>
            {
                new object[] { new User() { Username = "", PasswordHash = "hash", Email = "test@test.com" } },
                new object[] { new User() { Username = null, PasswordHash = "hash", Email = "test@test.com" } },
                new object[] { new User() { Username = "user", PasswordHash = "", Email = "test@test.com" } },
                new object[] { new User() { Username = "user", PasswordHash = null, Email = "test@test.com" } },
                new object[] { new User() { Username = "user", PasswordHash = "hash", Email = "" } },
                new object[] { new User() { Username = "user", PasswordHash = "hash", Email = null } }
            };
        }
    }
}
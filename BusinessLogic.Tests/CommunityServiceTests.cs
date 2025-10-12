using Moq;
using BusinessLogic.Services;
using Domain.Interfaces.Repositories;
using Domain.Wrapper;
using Domain.Models;
using Xunit;

namespace BusinessLogic.Tests
{
    public class CommunityServiceTests
    {
        private readonly CommunityService _service;
        private readonly Mock<ICommunityRepository> _communityRepositoryMoq;
        private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public CommunityServiceTests()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _communityRepositoryMoq = new Mock<ICommunityRepository>();
            _subscriptionRepositoryMoq = new Mock<ISubscriptionRepository>();

            _repositoryWrapperMoq.Setup(x => x.Community).Returns(_communityRepositoryMoq.Object);
            _repositoryWrapperMoq.Setup(x => x.Subscription).Returns(_subscriptionRepositoryMoq.Object);

            _service = new CommunityService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidCommunity_ShouldCreateCommunity()
        {
            // Arrange
            var community = new Community
            {
                Name = "Test Community",
                Description = "Test Description",
                CreatorId = Guid.NewGuid()
            };

            // Act
            var result = await _service.Create(community);

            // Assert
            _communityRepositoryMoq.Verify(x => x.Create(community), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(community, result);
        }

        [Fact]
        public async Task UpdateAsync_ValidCommunity_ShouldUpdateCommunity()
        {
            // Arrange
            var community = new Community
            {
                Id = Guid.NewGuid(),
                Name = "Updated Community",
                Description = "Updated Description"
            };

            // Act
            var result = await _service.Update(community);

            // Assert
            _communityRepositoryMoq.Verify(x => x.Update(community), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(community, result);
        }
    }
}
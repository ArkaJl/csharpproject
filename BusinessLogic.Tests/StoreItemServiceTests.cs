using Moq;
using BusinessLogic.Services;
using Domain.Interfaces.Repositories;
using Domain.Wrapper;
using Domain.Models;
using Xunit;

namespace BusinessLogic.Tests
{
    public class StoreItemServiceTests
    {
        private readonly StoreItemService _service;
        private readonly Mock<IStoreItemRepository> _storeItemRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public StoreItemServiceTests()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _storeItemRepositoryMoq = new Mock<IStoreItemRepository>();

            _repositoryWrapperMoq.Setup(x => x.StoreItem)
                .Returns(_storeItemRepositoryMoq.Object);

            _service = new StoreItemService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidStoreItem_ShouldCreateStoreItem()
        {
            // Arrange
            var storeItem = new StoreItem { Id = Guid.NewGuid(), Name = "Test Item", Price = 100 };

            // Act
            var result = await _service.Create(storeItem);

            // Assert
            _storeItemRepositoryMoq.Verify(x => x.Create(storeItem), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(storeItem, result);
        }

        [Fact]
        public async Task UpdateAsync_ValidStoreItem_ShouldUpdateStoreItem()
        {
            // Arrange
            var storeItem = new StoreItem { Id = Guid.NewGuid(), Name = "Updated Item", Price = 150 };

            // Act
            var result = await _service.Update(storeItem);

            // Assert
            _storeItemRepositoryMoq.Verify(x => x.Update(storeItem), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(storeItem, result);
        }
    }
}
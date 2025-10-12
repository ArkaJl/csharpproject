using Moq;
using BusinessLogic.Services;
using Domain.Interfaces.Repositories;
using Domain.Wrapper;
using Domain.Models;

namespace BusinessLogic.Tests
{
    public class AlbumServiceTests
    {
        private readonly AlbumService _service;
        private readonly Mock<IAlbumRepository> _albumRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public AlbumServiceTests()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _albumRepositoryMoq = new Mock<IAlbumRepository>();

            _repositoryWrapperMoq.Setup(x => x.Album)
                .Returns(_albumRepositoryMoq.Object);

            _service = new AlbumService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidAlbum_ShouldCreateAlbum()
        {
            // Arrange
            var album = new Album { Id = Guid.NewGuid(), Name = "Test Album" };

            // Act
            var result = await _service.Create(album);

            // Assert
            _albumRepositoryMoq.Verify(x => x.Create(album), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(album, result);
        }

        [Fact]
        public async Task UpdateAsync_ValidAlbum_ShouldUpdateAlbum()
        {
            // Arrange
            var album = new Album { Id = Guid.NewGuid(), Name = "Updated Album" };

            // Act
            var result = await _service.Update(album);

            // Assert
            _albumRepositoryMoq.Verify(x => x.Update(album), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(album, result);
        }
    }
}
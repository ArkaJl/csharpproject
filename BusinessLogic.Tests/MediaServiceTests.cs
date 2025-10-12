using Moq;
using BusinessLogic.Services;
using Domain.Interfaces.Repositories;
using Domain.Wrapper;
using Domain.Models;

namespace BusinessLogic.Tests
{
    public class MediaServiceTests
    {
        private readonly MediaService _service;
        private readonly Mock<IMediaRepository> _mediaRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public MediaServiceTests()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _mediaRepositoryMoq = new Mock<IMediaRepository>();
            _repositoryWrapperMoq.Setup(x => x.Media).Returns(_mediaRepositoryMoq.Object);
            _service = new MediaService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidMedia_ShouldCreateMedia()
        {
            // Arrange
            var media = new Medium
            {
                Type = "image",
                UploadedBy = Guid.NewGuid()
            };

            // Act
            var result = await _service.Create(media);

            // Assert
            _mediaRepositoryMoq.Verify(x => x.Create(media), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(media, result);
        }

        [Fact]
        public async Task UpdateAsync_ValidMedia_ShouldUpdateMedia()
        {
            // Arrange
            var media = new Medium
            {
                Id = Guid.NewGuid(),
                Type = "image"
            };

            // Act
            var result = await _service.Update(media);

            // Assert
            _mediaRepositoryMoq.Verify(x => x.Update(media), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(media, result);
        }

        [Fact]
        public async Task CreateAsync_MediaWithEmptyFileName_ShouldCreateMedia()
        {
            // Arrange
            var media = new Medium
            {
                Type = "image",
                UploadedBy = Guid.NewGuid()
            };

            // Act
            var result = await _service.Create(media);

            // Assert
            _mediaRepositoryMoq.Verify(x => x.Create(media), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}
using Moq;
using BusinessLogic.Services;
using Domain.Interfaces.Repositories;
using Domain.Wrapper;
using Domain.Models;
using Xunit;

namespace BusinessLogic.Tests
{
    public class MessageServiceTests
    {
        private readonly MessageService _service;
        private readonly Mock<IMessageRepository> _messageRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public MessageServiceTests()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _messageRepositoryMoq = new Mock<IMessageRepository>();
            _repositoryWrapperMoq.Setup(x => x.Message).Returns(_messageRepositoryMoq.Object);
            _service = new MessageService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidMessage_ShouldCreateMessage()
        {
            // Arrange
            var message = new Message
            {
                Content = "Test message content",
                SenderId = Guid.NewGuid(),
                ChatId = Guid.NewGuid()
            };

            // Act
            var result = await _service.Create(message);

            // Assert
            _messageRepositoryMoq.Verify(x => x.Create(message), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(message, result);
        }

        [Fact]
        public async Task CreateAsync_MessageWithEmptyContent_ShouldCreateMessage()
        {
            // Arrange
            var message = new Message
            {
                Content = "",
                SenderId = Guid.NewGuid(),
                ChatId = Guid.NewGuid()
            };

            // Act
            var result = await _service.Create(message);

            // Assert
            _messageRepositoryMoq.Verify(x => x.Create(message), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidMessage_ShouldUpdateMessage()
        {
            // Arrange
            var message = new Message
            {
                Id = Guid.NewGuid(),
                Content = "Updated message content",
                SenderId = Guid.NewGuid(),
                ChatId = Guid.NewGuid()
            };

            // Act
            var result = await _service.Update(message);

            // Assert
            _messageRepositoryMoq.Verify(x => x.Update(message), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(message, result);
        }
    }
}
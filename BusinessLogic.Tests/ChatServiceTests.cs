using Moq;
using BusinessLogic.Services;
using Domain.Interfaces.Repositories;
using Domain.Wrapper;
using Domain.Models;
using Xunit;

namespace BusinessLogic.Tests
{
    public class ChatServiceTests
    {
        private readonly ChatService _service;
        private readonly Mock<IChatRepository> _chatRepositoryMoq;
        private readonly Mock<IChatParticipantRepository> _chatParticipantRepositoryMoq;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;

        public ChatServiceTests()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _chatRepositoryMoq = new Mock<IChatRepository>();
            _chatParticipantRepositoryMoq = new Mock<IChatParticipantRepository>();

            _repositoryWrapperMoq.Setup(x => x.Chat).Returns(_chatRepositoryMoq.Object);
            _repositoryWrapperMoq.Setup(x => x.ChatParticipant).Returns(_chatParticipantRepositoryMoq.Object);

            _service = new ChatService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidChat_ShouldCreateChat()
        {
            // Arrange
            var chat = new Chat { Id = Guid.NewGuid(), Name = "Test Chat" };

            // Act
            var result = await _service.Create(chat);

            // Assert
            _chatRepositoryMoq.Verify(x => x.Create(chat), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
            Assert.Equal(chat, result);
        }

        [Fact]
        public async Task AddParticipantAsync_ValidData_ShouldAddParticipant()
        {
            // Arrange
            var chatId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            // Act
            await _service.AddParticipant(chatId, userId);

            // Assert
            _chatParticipantRepositoryMoq.Verify(x => x.Create(It.Is<ChatParticipant>(cp =>
                cp.ChatId == Guid.Parse(chatId) &&
                cp.UserId == Guid.Parse(userId))), Times.Once);
            _repositoryWrapperMoq.Verify(x => x.SaveAsync(), Times.Once);
        }
    }
}
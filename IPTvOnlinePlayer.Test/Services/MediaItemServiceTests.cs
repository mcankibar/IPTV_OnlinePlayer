using FluentAssertions;
using IPTvOnlinePlayer.Domain.Playlists;
using IPTvOnlinePlayer.Services;
using IPTvOnlinePlayer.Services.Playlists;
using Moq;

namespace IPTvOnlinePlayer.Test.Services;

[TestFixture]
public class MediaItemServiceTests
{
    private Mock<IRepository<MediaItem>> _mediaItemRepositoryMock;
    private IMediaItemService _mediaItemService;

    [SetUp]
    public void SetUp()
    {
        _mediaItemRepositoryMock = new Mock<IRepository<MediaItem>>();
        _mediaItemService = new MediaItemService(_mediaItemRepositoryMock.Object);
    }

    [Test]
    public async Task GetMediaItemByIdAsync_ShouldReturnMediaItem()
    {
        // Arrange
        var mediaItem = new MediaItem { Id = 1 };
        _mediaItemRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mediaItem);

        // Act
        var result = await _mediaItemService.GetMediaItemByIdAsync(1);

        // Assert
        result.Should().BeEquivalentTo(mediaItem);
    }

    [Test]
    public async Task GetAllMediaItemsAsync_ShouldReturnAllMediaItems()
    {
        // Arrange
        var mediaItems = new List<MediaItem> { new MediaItem { Id = 1 }, new MediaItem { Id = 2 } };
        _mediaItemRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mediaItems);

        // Act
        var result = await _mediaItemService.GetAllMediaItemsAsync();

        // Assert
        result.Should().BeEquivalentTo(mediaItems);
    }

    [Test]
    public async Task InsertMediaItemAsync_ShouldInsertMediaItem()
    {
        // Arrange
        var mediaItem = new MediaItem { Id = 1 };

        // Act
        await _mediaItemService.InsertMediaItemAsync(mediaItem);

        // Assert
        _mediaItemRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MediaItem>()), Times.Once);
    }

    [Test]
    public async Task InsertMediaItemsAsync_ShouldInsertMediaItems()
    {
        // Arrange
        var mediaItems = new List<MediaItem> { new MediaItem { Id = 1 }, new MediaItem { Id = 2 } };

        // Act
        await _mediaItemService.InsertMediaItemsAsync(mediaItems);

        // Assert
        _mediaItemRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MediaItem>()), Times.Exactly(mediaItems.Count));
    }

    [Test]
    public async Task UpdateMediaItemAsync_ShouldUpdateMediaItem()
    {
        // Arrange
        var mediaItem = new MediaItem { Id = 1 };

        // Act
        await _mediaItemService.UpdateMediaItemAsync(mediaItem);

        // Assert
        _mediaItemRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MediaItem>()), Times.Once);
    }

    [Test]
    public async Task DeleteMediaItemAsync_ShouldDeleteMediaItem()
    {
        // Arrange
        var mediaItem = new MediaItem { Id = 1 };
        _mediaItemRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mediaItem);

        // Act
        await _mediaItemService.DeleteMediaItemAsync(1);

        // Assert
        _mediaItemRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
    }
}
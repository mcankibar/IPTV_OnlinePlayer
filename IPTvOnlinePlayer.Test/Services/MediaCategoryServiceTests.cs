using FluentAssertions;
using IPTvOnlinePlayer.Domain.Playlists;
using IPTvOnlinePlayer.Services;
using IPTvOnlinePlayer.Services.Playlists;
using Moq;

namespace IPTvOnlinePlayer.Test.Services;

[TestFixture]
public class MediaCategoryServiceTests
{
    private Mock<IRepository<MediaCategory>> _mediaCategoryRepositoryMock;
    private IMediaCategoryService _mediaCategoryService;

    [SetUp]
    public void SetUp()
    {
        _mediaCategoryRepositoryMock = new Mock<IRepository<MediaCategory>>();
        _mediaCategoryService = new MediaCategoryService(_mediaCategoryRepositoryMock.Object);
    }

    [Test]
    public async Task GetMediaCategoryByIdAsync_ShouldReturnMediaCategory()
    {
        // Arrange
        var mediaCategory = new MediaCategory { Id = 1 };
        _mediaCategoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mediaCategory);

        // Act
        var result = await _mediaCategoryService.GetMediaCategoryByIdAsync(1);

        // Assert
        result.Should().BeEquivalentTo(mediaCategory);
    }

    [Test]
    public async Task GetAllMediaCategoriesAsync_ShouldReturnAllMediaCategories()
    {
        // Arrange
        var mediaCategories = new List<MediaCategory> { new MediaCategory { Id = 1 }, new MediaCategory { Id = 2 } };
        _mediaCategoryRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mediaCategories);

        // Act
        var result = await _mediaCategoryService.GetAllMediaCategoriesAsync();

        // Assert
        result.Should().BeEquivalentTo(mediaCategories);
    }

    [Test]
    public async Task InsertMediaCategoryAsync_ShouldInsertMediaCategory()
    {
        // Arrange
        var mediaCategory = new MediaCategory { Id = 1 };

        // Act
        await _mediaCategoryService.InsertMediaCategoryAsync(mediaCategory);

        // Assert
        _mediaCategoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MediaCategory>()), Times.Once);
    }

    [Test]
    public async Task InsertMediaCategoriesAsync_ShouldInsertMediaCategories()
    {
        // Arrange
        var mediaCategories = new List<MediaCategory> { new MediaCategory { Id = 1 }, new MediaCategory { Id = 2 } };

        // Act
        await _mediaCategoryService.InsertMediaCategoriesAsync(mediaCategories);

        // Assert
        _mediaCategoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<MediaCategory>()), Times.Exactly(mediaCategories.Count));
    }

    [Test]
    public async Task UpdateMediaCategoryAsync_ShouldUpdateMediaCategory()
    {
        // Arrange
        var mediaCategory = new MediaCategory { Id = 1 };

        // Act
        await _mediaCategoryService.UpdateMediaCategoryAsync(mediaCategory);

        // Assert
        _mediaCategoryRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<MediaCategory>()), Times.Once);
    }

    [Test]
    public async Task DeleteMediaCategoryAsync_ShouldDeleteMediaCategory()
    {
        // Arrange
        var mediaCategory = new MediaCategory { Id = 1 };
        _mediaCategoryRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mediaCategory);

        // Act
        await _mediaCategoryService.DeleteMediaCategoryAsync(1);

        // Assert
        _mediaCategoryRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
    }
}
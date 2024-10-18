using FluentAssertions;
using IPTvOnlinePlayer.Domain.Playlists;
using IPTvOnlinePlayer.Services;
using IPTvOnlinePlayer.Services.Playlists;
using Moq;

namespace IPTvOnlinePlayer.Test.Services
{
    [TestFixture]
    public class PlaylistServiceTests
    {
        private Mock<IRepository<Playlist>> _playlistRepositoryMock;
        private IPlaylistService _playlistService;

        [SetUp]
        public void SetUp()
        {
            _playlistRepositoryMock = new Mock<IRepository<Playlist>>();
            _playlistService = new PlaylistService(_playlistRepositoryMock.Object);
        }

        [Test]
        public async Task GetPlaylistByIdAsync_ShouldReturnPlaylist()
        {
            // Arrange
            var playlist = new Playlist { Id = 1 };
            _playlistRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(playlist);

            // Act
            var result = await _playlistService.GetPlaylistByIdAsync(1);
            
            // Assert
            result.Should().BeEquivalentTo(playlist);
        }

        [Test]
        public async Task GetAllPlaylistsAsync_ShouldReturnAllPlaylists()
        {
            // Arrange
            var playlists = new List<Playlist> { new Playlist { Id = 1 }, new Playlist { Id = 2 } };
            _playlistRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(playlists);

            // Act
            var result = await _playlistService.GetAllPlaylistsAsync();

            // Assert
            result.Should().BeEquivalentTo(playlists);
        }

        [Test]
        public async Task InsertPlaylistAsync_ShouldInsertPlaylist()
        {
            // Arrange
            var playlist = new Playlist { Id = 1 };

            // Act
            await _playlistService.InsertPlaylistAsync(playlist);

            // Assert
            _playlistRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Playlist>()), Times.Once);
        }

        [Test]
        public async Task InsertPlaylistsAsync_ShouldInsertPlaylists()
        {
            // Arrange
            var playlists = new List<Playlist> { new Playlist { Id = 1 }, new Playlist { Id = 2 } };

            // Act
            await _playlistService.InsertPlaylistsAsync(playlists);

            // Assert
            _playlistRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Playlist>()), Times.Exactly(playlists.Count));
        }

        [Test]
        public async Task UpdatePlaylistAsync_ShouldUpdatePlaylist()
        {
            // Arrange
            var playlist = new Playlist { Id = 1 };

            // Act
            await _playlistService.UpdatePlaylistAsync(playlist);

            // Assert
            _playlistRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Playlist>()), Times.Once);
        }

        [Test]
        public async Task DeletePlaylistAsync_ShouldDeletePlaylist()
        {
            // Arrange
            var playlist = new Playlist { Id = 1 };
            _playlistRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(playlist);

            // Act
            await _playlistService.DeletePlaylistAsync(1);

            // Assert
            _playlistRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
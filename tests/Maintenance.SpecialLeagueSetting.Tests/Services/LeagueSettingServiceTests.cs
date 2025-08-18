using Fanex.Data.Repository;
using Fanex.RemotingRef;
using Maintenance.SpecialLeagueSetting.Models;
using Maintenance.SpecialLeagueSetting.Request;
using Maintenance.SpecialLeagueSetting.Services;
using NSubstitute;

namespace Maintenance.SpecialLeagueSetting.Tests.Services
{
    public class LeagueSettingServiceTests
    {
        [Fact]
        public async Task GetLeagueSetting_ReturnsLeagueWithNames()
        {
            // Arrange
            var repository = Substitute.For<IDynamicRepository>();
            var remotingRefService = Substitute.For<IRemotingRefService>();
            var leagues = new List<League>
            {
                new League { LeagueId = 1, SportType = 2 },
                new League { LeagueId = 2, SportType = 3 }
            };
            repository.FetchAsync<League>(Arg.Any<ICriteria>()).Returns(leagues);
            remotingRefService.GetLeagueName("1", "en", string.Empty).Returns("League1");
            remotingRefService.GetLeagueName("2", "en", string.Empty).Returns("League2");
            remotingRefService.GetSportName("2", "en", string.Empty).Returns("Soccer");
            remotingRefService.GetSportName("3", "en", string.Empty).Returns("Basketball");
            var service = new LeagueSettingServiceImp(repository, remotingRefService);
            var request = new GetLeagueSetting { AutoType = 1, LeagueId = 1 };

            // Act
            var result = await service.GetLeagueSetting(request);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("League1", result[0].LeagueName);
            Assert.Equal("Soccer", result[0].SportName);
            Assert.Equal("League2", result[1].LeagueName);
            Assert.Equal("Basketball", result[1].SportName);
        }

        [Fact]
        public async Task UpdateLeagueSetting_CallsRepositoryAndReturnsTrue()
        {
            // Arrange
            var repository = Substitute.For<IDynamicRepository>();
            var remotingRefService = Substitute.For<IRemotingRefService>();
            await repository.ExecuteAsync(Arg.Any<INonQueryCommand>());
            var service = new LeagueSettingServiceImp(repository, remotingRefService);
            var request = new UpdateSettingRequest { LeagueId = 1, AutoType = 1, SportType = 2 };

            // Act
            var result = await service.UpdateLeagueSetting(request);

            // Assert
            await repository.Received().ExecuteAsync(Arg.Any<INonQueryCommand>());
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveLeagueSetting_CallsRepositoryAndReturnsTrue()
        {
            // Arrange
            var repository = Substitute.For<IDynamicRepository>();
            var remotingRefService = Substitute.For<IRemotingRefService>();
            await repository.ExecuteAsync(Arg.Any<INonQueryCommand>());
            var service = new LeagueSettingServiceImp(repository, remotingRefService);
            var request = new RemoveSettingRequest { LeagueId = 1, AutoType = 1 };

            // Act
            var result = await service.RemoveLeagueSetting(request);

            // Assert
            await repository.Received().ExecuteAsync(Arg.Any<INonQueryCommand>());
            Assert.True(result);
        }
    }
}

using Core.Models;
using Fanex.Caching;
using Fanex.Logging;
using Fanex.NAP.Client;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using ONEbook.UM.Services;
using StackExchange.Redis;

namespace Maintenance.DeleteTempVariable.Tests.Services
{
    public class FlagBroadcastServiceTests
    {

        [Fact]
        public async Task UpdateFlagInfo_WithRequest_SetsCacheAndLogs()
        {
            // Arrange
            var cache = Substitute.For<ICache>();
            var logger = Substitute.For<ILogger>();
            var session = Substitute.For<INapSession>();
            session.UserName.Returns("user");
            session.UserId.Returns(123);
            var maintenanceService = Substitute.For<IMaintenanceService>();
            var connectionMultiplexer = Substitute.For<IConnectionMultiplexer>();
            var appSettings = new AppSettingsKey();
            var options = Substitute.For<IOptions<AppSettingsKey>>();
            options.Value.Returns(appSettings);
            var service = new FlagBroadcastServiceImp(connectionMultiplexer, cache, logger, session, options, maintenanceService);
            var request = new UpdateFlagBroadcastRequest { Status = true, ProductType = new List<int> { 1, 2 } };
            // Act
            var result = await service.UpdateFlagInfo(request);
            // Assert
            await cache.Received().SetAsync(CacheKey.BroadcastProductCacheKey, "1,2", Arg.Any<CacheItemOptions>());
            await cache.Received().SetAsync(CacheKey.BroadcastFlagCacheKey, true, Arg.Any<CacheItemOptions>());
            await logger.Received().InfoAsync(Arg.Is<string>(msg => msg.Contains("update flag with value True")));
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateFlagInfo_WithBool_SetsCacheAndLogs()
        {
            // Arrange
            var cache = Substitute.For<ICache>();
            var logger = Substitute.For<ILogger>();
            var session = Substitute.For<INapSession>();
            session.UserName.Returns("user");
            session.UserId.Returns(123);
            var maintenanceService = Substitute.For<IMaintenanceService>();
            var connectionMultiplexer = Substitute.For<IConnectionMultiplexer>();
            var appSettings = new AppSettingsKey();
            var options = Substitute.For<IOptions<AppSettingsKey>>();
            options.Value.Returns(appSettings);
            var service = new FlagBroadcastServiceImp(connectionMultiplexer, cache, logger, session, options, maintenanceService);
            // Act
            var result = await service.UpdateFlagInfo(true);
            // Assert
            await cache.Received().SetAsync(CacheKey.BroadcastSettleUnderMaintenanceCacheKey, true, Arg.Any<CacheItemOptions>());
            await logger.Received().InfoAsync(Arg.Is<string>(msg => msg.Contains("update flag with value True")));
            Assert.True(result);
        }
    }
}

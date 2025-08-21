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

        [Fact]
        public async Task UpdateFlagInfo_WithFalseStatus_SetsCacheAndLogs()
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
            var request = new UpdateFlagBroadcastRequest { Status = false, ProductType = new List<int>() };

            // Act
            var result = await service.UpdateFlagInfo(request);

            // Assert
            await cache.Received().SetAsync(CacheKey.BroadcastProductCacheKey, string.Empty, Arg.Any<CacheItemOptions>());
            await cache.Received().SetAsync(CacheKey.BroadcastFlagCacheKey, false, Arg.Any<CacheItemOptions>());
            await logger.Received().InfoAsync(Arg.Is<string>(msg => msg.Contains("update flag with value False")));
            Assert.False(result);
        }

        [Fact]
        public async Task GetFlagInfo_HandlesCacheMisses()
        {
            // Arrange
            var cache = Substitute.For<ICache>();
            var logger = Substitute.For<ILogger>();
            var session = Substitute.For<INapSession>();
            var maintenanceService = Substitute.For<IMaintenanceService>();
            var connectionMultiplexer = Substitute.For<IConnectionMultiplexer>();
            var db = Substitute.For<IDatabase>();
            var appSettings = new AppSettingsKey
            {
                RabbitMq = new RabbitMq { Hosting = "host", VirtualHost = "vhost", Username = "user", Password = "pass" }
            };
            var options = Substitute.For<IOptions<AppSettingsKey>>();
            options.Value.Returns(appSettings);
            connectionMultiplexer.GetDatabase().Returns(db);
            db.ListLengthAsync(Arg.Any<RedisKey>()).Returns(1);
            cache.ContainAsync(Arg.Any<string>()).Returns(false);
            cache.SetAsync(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<CacheItemOptions>()).Returns(Task.CompletedTask);
            cache.TryGetAsync<bool>(Arg.Any<string>()).Returns((true, false));
            cache.TryGetAsync<string>(Arg.Any<string>()).Returns((true, ""));
            maintenanceService.IsUnderMaintenanceAsync().Returns(false);
            // Use partial substitute to mock the RabbitMQ method
            var service = Substitute.ForPartsOf<FlagBroadcastServiceImp>(connectionMultiplexer, cache, logger, session, options, maintenanceService);
            service.When(x => x.GetNumberMessageProcessingQueueAsync()).DoNotCallBase();
            service.GetNumberMessageProcessingQueueAsync().Returns(Task.FromResult(1u));

            // Act
            var result = await service.GetFlagInfo();

            // Assert
            Assert.False(result.CurrentStatus);
            Assert.Equal("", result.ProductType);
            Assert.False(result.IsUnderMaintenance);
        }

        [Fact]
        public void GetListMessageWaiting_ReturnsFormattedHtml()
        {
            // Arrange
            var connectionMultiplexer = Substitute.For<IConnectionMultiplexer>();
            var db = Substitute.For<IDatabase>();
            connectionMultiplexer.GetDatabase().Returns(db);
            var cache = Substitute.For<ICache>();
            var logger = Substitute.For<ILogger>();
            var session = Substitute.For<INapSession>();
            var maintenanceService = Substitute.For<IMaintenanceService>();
            var appSettings = new AppSettingsKey();
            var options = Substitute.For<IOptions<AppSettingsKey>>();
            options.Value.Returns(appSettings);
            var service = new FlagBroadcastServiceImp(connectionMultiplexer, cache, logger, session, options, maintenanceService);
            var redisValues = new RedisValue[] { "A:space:B", "C:space:D" };
            db.ListRange(Arg.Any<RedisKey>()).Returns(redisValues);

            // Act
            var method = typeof(FlagBroadcastServiceImp).GetMethod("GetListMessageWaiting", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (string)method.Invoke(service, new object[] { "key" });

            // Assert
            Assert.Contains("Product Type: A", result);
            Assert.Contains("Product Type: C", result);
            Assert.Contains("B", result);
            Assert.Contains("D", result);
        }
    }
}

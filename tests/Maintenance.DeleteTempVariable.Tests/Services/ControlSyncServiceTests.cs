using Core.Models;
using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.NAP.Client;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

public class ControlSyncServiceTests
{
    private readonly ICache cache;
    private readonly ILogger logger;
    private readonly INapSession session;
    private readonly IDynamicRepository repository;
    private readonly IOptions<AppSettingsKey> appSettingsKey;

    public ControlSyncServiceTests()
    {
        var settings = new AppSettingsKey
        {
            SiteDbServer = ["ABC", "DEF"]
        };
        appSettingsKey = Options.Create(settings);
        session = Substitute.For<INapSession>();
        logger = Substitute.For<ILogger>();
        cache = Substitute.For<ICache>();
        repository = Substitute.For<IDynamicRepository>();
    }

    [Fact]
    public async Task GetInfoService_ReturnCorrectData()
    {
        // Arrange
        var service = new ControlSyncServicesImp(cache, logger, session, appSettingsKey, repository);
        repository.FetchAsync<LastLog>(Arg.Any<GetServiceLastLogCriteria>()).Returns(new List<LastLog>());

        // Act
        var result = await service.GetInfoService();

        // Assert
        Assert.Equal(result, []);
    }

    [Fact]
    public void UpdateStatusService_UpdatesCacheAndLogs()
    {
        // Arrange
        var service = new ControlSyncServicesImp(cache, logger, session, appSettingsKey, repository);
        var request = new UpdateStatusRequest { Status = true, CacheKey = "key" };

        // Act
        var result = service.UpdateStatusService(request);

        // Assert
        cache.Received().Set(request.CacheKey, request.Status, Arg.Any<CacheItemOptions>());
        logger.Received().Info(Arg.Is<string>(msg => msg.Contains("update status with value True")));
        Assert.True(result);
    }

    [Fact]
    public async Task GetInfoService_MultipleServers_ReturnsLogsWithCorrectDbServer()
    {
        // Arrange
        var service = new ControlSyncServicesImp(cache, logger, session, appSettingsKey, repository);
        var logsABC = new List<LastLog> { new LastLog { QueueType = QueueType.SportBook } };
        var logsDEF = new List<LastLog> { new LastLog { QueueType = QueueType.NonSportBook } };
        repository.FetchAsync<LastLog>(Arg.Is<GetServiceLastLogCriteria>(c => c.DbServer == "ABC")).Returns(logsABC);
        repository.FetchAsync<LastLog>(Arg.Is<GetServiceLastLogCriteria>(c => c.DbServer == "DEF")).Returns(logsDEF);
        cache.Contain(Arg.Any<string>()).Returns(true);
        cache.TryGet(Arg.Any<string>(), out Arg.Any<bool>()).Returns(x => { x[1] = true; return true; });

        // Act
        var result = await service.GetInfoService();

        // Assert
        Assert.Contains(result, l => l.DbServer == "ABC");
        Assert.Contains(result, l => l.DbServer == "DEF");
        Assert.All(result, l => Assert.True(l.CurrentStatus));
    }

    [Fact]
    public void GetStatusFromCache_CacheMiss_SetsCacheAndReturnsTrue()
    {
        // Arrange
        var service = new ControlSyncServicesImp(cache, logger, session, appSettingsKey, repository);
        cache.Contain("key").Returns(false);
        cache.TryGet("key", out Arg.Any<bool>()).Returns(x => { x[1] = true; return true; });

        // Act
        var method = typeof(ControlSyncServicesImp).GetMethod("GetStatusFromCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = (bool)method.Invoke(service, new object[] { "key" });

        // Assert
        cache.Received().Set("key", true, Arg.Any<CacheItemOptions>());
        Assert.True(result);
    }

    [Fact]
    public void GetStatusFromCache_CacheHit_ReturnsStatus()
    {
        // Arrange
        var service = new ControlSyncServicesImp(cache, logger, session, appSettingsKey, repository);
        cache.Contain("key").Returns(true);
        cache.TryGet("key", out Arg.Any<bool>()).Returns(x => { x[1] = false; return true; });

        // Act
        var method = typeof(ControlSyncServicesImp).GetMethod("GetStatusFromCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = (bool)method.Invoke(service, new object[] { "key" });

        // Assert
        Assert.False(result);
    }
}
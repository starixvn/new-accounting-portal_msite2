using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.NAP.Client;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Maintenance.DeleteTempVariable.Tests.Services;

using Core.Models;

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
            SiteDbServer = ["ABC"]
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
}
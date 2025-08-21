using Core.Constants;
using Core.Models;
using Fanex.Data.Repository;
using Maintenance.Sync.Criteria;
using Maintenance.Sync.Requests;
using Maintenance.Sync.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Maintenance.Sync.Tests.Services;

public class SyncTransDirectlyServiceTests
{
    private readonly IOptions<AppSettingsKey> appSettingsKey;
    private readonly IDynamicRepository repository;
    private readonly AppSettingsKey appSettings;

    public SyncTransDirectlyServiceTests()
    {
        appSettings = new AppSettingsKey
        {
            SiteDbServer = new List<string> { "DB1", "DB2", DataBaseServers.DBCentral },
            SubDbServer = new List<string> { "DB3" }
        };
        appSettingsKey = Substitute.For<IOptions<AppSettingsKey>>();
        appSettingsKey.Value.Returns(appSettings);
        repository = Substitute.For<IDynamicRepository>();
    }

    [Fact]
    public void GetDbInfo_ReturnsExpectedDbInfo()
    {
        var service = new SyncTransDirectlyServiceImp(appSettingsKey, repository);
        var dbInfo = service.GetDbInfo();
        Assert.Contains("bettrans", dbInfo.Tables);
        Assert.Contains("bettransm", dbInfo.Tables);
        Assert.Contains("DB1", dbInfo.DbSites);
        Assert.Contains("DB2", dbInfo.DbSites);
        Assert.DoesNotContain(DataBaseServers.DBCentral, dbInfo.DbSites);
        Assert.Contains(DataBaseServers.DBCentral, dbInfo.SubDbSites);
        Assert.Contains("DB3", dbInfo.SubDbSites);
    }

    [Fact]
    public async Task UpdateTransId_CentralDb_ReturnsResultFromCentralDb()
    {
        var service = new SyncTransDirectlyServiceImp(appSettingsKey, repository);
        var request = new UpdateTransIdsRequest
        {
            TableName = "bettrans",
            TransIds = "1,2,3",
            ServerName = DataBaseServers.DBCentral,
            ToServerNames = new[] { DataBaseServers.DBCentral }
        };
        await repository.ExecuteAsync(Arg.Any<SyncTransIdToCentralDbUpdCriteria>());
        var result = await service.UpdateTransId(request);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateTransId_AllSites_ReturnsSuccessResult()
    {
        var service = new SyncTransDirectlyServiceImp(appSettingsKey, repository);
        var request = new UpdateTransIdsRequest
        {
            TableName = "bettrans",
            TransIds = "1,2,3",
            ServerName = "DB1",
            ToServerNames = new[] { "DB1", "DB2" }
        };
        await repository.ExecuteAsync(Arg.Any<SyncTransIdSelCriteria>());
        await repository.ExecuteAsync(Arg.Any<SyncTransIdUpdCriteria>());
        var result = await service.UpdateTransId(request);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateTransId_GetTransIdsFails_ReturnsErrorResult()
    {
        var service = new SyncTransDirectlyServiceImp(appSettingsKey, repository);
        var request = new UpdateTransIdsRequest
        {
            TableName = "bettrans",
            TransIds = "1,2,3",
            ServerName = "DB1",
            ToServerNames = new[] { "DB1", "DB2" }
        };
        // Simulate error by setting ErrCode on criteria
        await repository.ExecuteAsync(Arg.Do<SyncTransIdSelCriteria>(c => c.ErrCode = 99));
        var result = await service.UpdateTransId(request);
        Assert.Equal(99, result.ErrCode);
    }
}

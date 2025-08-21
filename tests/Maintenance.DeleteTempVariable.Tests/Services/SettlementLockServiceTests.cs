using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Maintenance.DeleteTempVariable.Tests.Services;

using Core.Models;

public class SettlementLockServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly IOptions<AppSettingsKey> appSettingsKey;
    private readonly IMapper mapper;

    public SettlementLockServiceTests()
    {
        var settings = new AppSettingsKey
        {
            SiteDbServer = ["ABC"]
        };
        appSettingsKey = Options.Create(settings);

        repository = Substitute.For<IDynamicRepository>();

        mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public async Task GetMatches_ReturnCorrectData()
    {
        // Arrange
        var service = new SettlementLockServiceImp(repository, appSettingsKey, mapper);
        repository.FetchAsync<MatchSettlementLock>(Arg.Any<GetParlayLockingCriteria>()).Returns(new List<MatchSettlementLock>());

        // Act
        var result = await service.GetMatches(new MatchRequest());

        // Assert
        Assert.Equal([], result);
    }

    [Fact]
    public async Task UpdateSystemSetting_UpdateSuccess()
    {
        // Arrange
        var service = new SettlementLockServiceImp(repository, appSettingsKey, mapper);
        repository.ExecuteAsync(Arg.Any<UpdateSettlementLogCriteria>()).Returns(1);

        // Act
        var result = await service.UpdateSettlementLogs(new UpdateSettlementLogRequest());

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task Delete_DeleteSuccess()
    {
        // Arrange
        var service = new SettlementLockServiceImp(repository, appSettingsKey, mapper);
        repository.ExecuteAsync(Arg.Any<DeleteSettlementLockCriteria>()).Returns(1);

        // Act
        var result = await service.Delete(new DeleteSettlementProcessingRequest());

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetMatches_MultipleServersAndReplicatedData_ReturnsAllLocksWithCorrectFlags()
    {
        // Arrange
        var settings = new AppSettingsKey
        {
            SiteDbServer = new List<string> { "DB1", "DB2", Core.Constants.DataBaseServers.DBCentral },
            SubDbServer = new List<string> { "DB3", "DB4" }
        };
        var appSettingsKey = Options.Create(settings);
        var repository = Substitute.For<IDynamicRepository>();
        var mapper = Substitute.For<IMapper>();
        var service = new SettlementLockServiceImp(repository, appSettingsKey, mapper);
        var matchRequest = new MatchRequest { MatchId = 10, Mode = "A" };
        // Normal locks for each server
        repository.FetchAsync<MatchSettlementLock>(Arg.Is<GetSettlementLockCriteria>(c => c.DbServer == "DB1"))
            .Returns(new List<MatchSettlementLock> { new MatchSettlementLock { MatchId = 10, DbServer = "DB1" } });

        repository.FetchAsync<MatchSettlementLock>(Arg.Is<GetSettlementLockCriteria>(c => c.DbServer == "DB2"))
            .Returns(new List<MatchSettlementLock> { new MatchSettlementLock { MatchId = 10, DbServer = "DB2" } });

        repository.FetchAsync<MatchSettlementLock>(Arg.Is<GetSettlementLockCriteria>(c => c.DbServer == Core.Constants.DataBaseServers.DBCentral))
            .Returns(new List<MatchSettlementLock> { new MatchSettlementLock { MatchId = 10, DbServer = Core.Constants.DataBaseServers.DBCentral } });

        // Replicated locks for each sub server
        repository.FetchAsync<MatchSettlementLock>(Arg.Is<GetSettlementLockReplicatedServerCriteria>(c => c.DbServer == "DB3"))
            .Returns(new List<MatchSettlementLock> { new MatchSettlementLock { MatchId = 10, DbServer = "DB3" } });

        repository.FetchAsync<MatchSettlementLock>(Arg.Is<GetSettlementLockReplicatedServerCriteria>(c => c.DbServer == "DB4"))
            .Returns(new List<MatchSettlementLock> { new MatchSettlementLock { MatchId = 10, DbServer = "DB4" } });

        // Act
        var result = await service.GetMatches(matchRequest);

        // Assert
        Assert.Equal(5, result.Count); // 3 normal + 2 replicated
        Assert.Contains(result, x => x.DbServer == "DB1" && !x.IsReplicated);
        Assert.Contains(result, x => x.DbServer == "DB2" && !x.IsReplicated);
        Assert.Contains(result, x => x.DbServer == Core.Constants.DataBaseServers.DBCentral && !x.IsReplicated);
        Assert.Contains(result, x => x.DbServer == "DB3" && x.IsReplicated);
        Assert.Contains(result, x => x.DbServer == "DB4" && x.IsReplicated);
    }
}
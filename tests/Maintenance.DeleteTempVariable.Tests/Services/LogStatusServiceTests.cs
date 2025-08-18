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

public class LogStatusServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly ScaleOutSettleService scaleOutSettleService;
    private readonly IOptions<AppSettingsKey> appSettingsKey;
    private readonly IMapper mapper;

    public LogStatusServiceTests()
    {
        var settings = new AppSettingsKey
        {
            SiteDbServer = ["ABC"]
        };
        appSettingsKey = Options.Create(settings);

        repository = Substitute.For<IDynamicRepository>();
        scaleOutSettleService = Substitute.For<ScaleOutSettleService>();
        mapper = Substitute.For<IMapper>();

    }

    [Fact]
    public async Task GetLogStatus_ReturnCorrectData()
    {
        // Arrange
        var service = new LogStatusServiceImp(repository, appSettingsKey, mapper, scaleOutSettleService);
        repository.FetchAsync<LogStatus>(Arg.Any<GetLogStatusCriteria>()).Returns(new List<LogStatus>());

        // Act
        var result = await service.GetLogStatus();

        // Assert
        Assert.Equal([], result);
    }

    [Fact]
    public async Task UpdateLogStatus_UpdateSuccess()
    {
        // Arrange
        var service = new LogStatusServiceImp(repository, appSettingsKey, mapper, scaleOutSettleService);
        repository.ExecuteAsync(Arg.Any<UpdateLogStatusCriteria>()).Returns(1);

        // Act
        var result = await service.UpdateLogStatus(Arg.Any<UpdateLogStatusRequest>());

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetLogStatus_ReturnsMultipleServersData()
    {
        // Arrange
        var settings = new AppSettingsKey
        {
            SiteDbServer = ["ABC", "DEF"]
        };
        var appSettingsKey = Options.Create(settings);
        var repository = Substitute.For<IDynamicRepository>();
        var scaleOutSettleService = Substitute.For<ScaleOutSettleService>();
        var mapper = Substitute.For<IMapper>();
        var service = new LogStatusServiceImp(repository, appSettingsKey, mapper, scaleOutSettleService);
        var logABC = new LogStatus { FunctionName = "FuncA" };
        var logDEF = new LogStatus { FunctionName = "FuncB" };
        repository.FetchAsync<LogStatus>(Arg.Is<GetLogStatusCriteria>(c => c.DbServer == "ABC")).Returns(new List<LogStatus> { logABC });
        repository.FetchAsync<LogStatus>(Arg.Is<GetLogStatusCriteria>(c => c.DbServer == "DEF")).Returns(new List<LogStatus> { logDEF });

        // Act
        var result = await service.GetLogStatus();

        // Assert
        Assert.Contains(result, x => x.FunctionName == "FuncA");
        Assert.Contains(result, x => x.FunctionName == "FuncB");
    }

    [Fact]
    public async Task UpdateLogStatus_MapsRequestAndExecutes()
    {
        // Arrange
        var service = new LogStatusServiceImp(repository, appSettingsKey, mapper, scaleOutSettleService);
        var request = new UpdateLogStatusRequest { DbServer = "ABC", LogIds = "1,2,3" };
        var criteria = new UpdateLogStatusCriteria();
        mapper.Map<UpdateLogStatusCriteria>(request).Returns(criteria);
        repository.ExecuteAsync(criteria).Returns(2);

        // Act
        var result = await service.UpdateLogStatus(request);

        // Assert
        Assert.Equal(2, result);
        await repository.Received().ExecuteAsync(criteria);
    }

    [Fact]
    public async Task CheckLogIdsInCentral_ReturnsIsPass()
    {
        // Arrange
        var service = new LogStatusServiceImp(repository, appSettingsKey, mapper, scaleOutSettleService);
        var request = new CheckLogIdsRequest { LogIds = "1,2,3", DbServer = "ABC" };
        scaleOutSettleService.GetSiteGroupId("ABC").Returns(99);
        repository.ExecuteAsync(Arg.Any<CheckLogIdsCriteria>()).Returns(0);

        // Act
        var result = await service.CheckLogIdsInCentral(request);

        // Assert
        Assert.True(result); // default IsPass is true
        await scaleOutSettleService.Received().GetSiteGroupId("ABC");
        await repository.Received().ExecuteAsync(Arg.Is<CheckLogIdsCriteria>(c => c.SiteDBGroupId == 99 && c.LogIds == "1,2,3"));
    }
}
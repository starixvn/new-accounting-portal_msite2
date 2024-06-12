using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Maintenance.DeleteTempVariable.Tests.Services;

public class SettlementLockServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly IOptions<AppSettingsKey> appSettingsKey;
    private readonly IMapper mapper;

    public SettlementLockServiceTests()
    {
        var settings = new AppSettingsKey
        {
            SiteDbServer = new List<string> { "ABC" }
        };
        appSettingsKey = Options.Create(settings);

        repository = Substitute.For<IDynamicRepository>();

        var mapperConfig = new MapperConfigurationExpression();
        mapperConfig.AddProfile<AutoMapperProfile>();
        var config = new MapperConfiguration(mapperConfig);
        mapper = new Mapper(config);
    }

    [Fact]
    public async void GetMatches_ReturnCorrectData()
    {
        // Arrange
        var service = new SettlementLockServiceImp(repository, appSettingsKey, mapper);
        repository.FetchAsync<MatchSettlementLock>(Arg.Any<GetParlayLockingCriteria>()).Returns(new List<MatchSettlementLock>());

        // Act
        var result = await service.GetMatches(new MatchRequest());

        // Assert
        Assert.Equal(new List<MatchSettlementLock>(), result);
    }

    [Fact]
    public async void UpdateSystemSetting_UpdateSuccess()
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
    public async void Delete_DeleteSuccess()
    {
        // Arrange
        var service = new SettlementLockServiceImp(repository, appSettingsKey, mapper);
        repository.ExecuteAsync(Arg.Any<DeleteSettlementLockCriteria>()).Returns(1);

        // Act
        var result = await service.Delete(new DeleteSettlementProcessingRequest());

        // Assert
        Assert.Equal(1, result);
    }
}
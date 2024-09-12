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

public class ParlayLockingServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly IOptions<AppSettingsKey> appSettingsKey;
    private readonly IMapper mapper;

    public ParlayLockingServiceTests()
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
        var service = new ParlayLockingServiceImp(repository, appSettingsKey, mapper);
        repository.FetchAsync<MatchParlayLocking>(Arg.Any<GetParlayLockingCriteria>()).Returns(new List<MatchParlayLocking>());

        // Act
        var result = await service.GetMatches(new ParlayLockingRequest());

        // Assert
        Assert.Equal(new List<MatchParlayLocking>(), result);
    }

    [Fact]
    public async void DeleteMatch_Success()
    {
        // Arrange
        var service = new ParlayLockingServiceImp(repository, appSettingsKey, mapper);
        repository.ExecuteAsync(Arg.Any<DeleteParlayLockingCriteria>()).Returns(1);

        // Act
        var result = await service.DeleteMatch(new DeleteParlayLockingRequest());

        // Assert
        Assert.Equal(1, result);
    }
}
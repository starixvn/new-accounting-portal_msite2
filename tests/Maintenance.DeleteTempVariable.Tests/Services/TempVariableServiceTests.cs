using AutoMapper;
using Fanex.Data.Repository;
using FluentAssertions;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Maintenance.DeleteTempVariable.Tests.Services;

public class TempVariableServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly IOptions<AppSettingsKey> appSettingsKey;
    private readonly IMapper mapper;

    public TempVariableServiceTests()
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
    public async void TicketCount_ReturnCount()
    {
        // Arrange 
        var service = new TempVariableServiceImp(repository, appSettingsKey, mapper);
        repository.GetAsync<MatchTicket>(Arg.Any<GetTicketCriteria>()).Returns(new MatchTicket { TicketCount = 1 });

        // Act
        var result = await service.TicketCount(new MatchTicketRequest());

        // Assert
        Assert.Equal(1, result);

    }

    [Fact]
    public async void GetMatches_ReturnCorrectData()
    {
        // Arrange 
        var service = new TempVariableServiceImp(repository, appSettingsKey, mapper);
        repository.FetchAsync<Match>(Arg.Any<GetTicketCriteria>()).Returns(new List<Match>());

        // Act
        var result = await service.GetMatches(1);

        // Assert
        Assert.Equal(new List<Match>(), result);

    }

    [Fact]
    public async void GetMatchDetail_ReturnCorrectData()
    {
        // Arrange 
        var service = new TempVariableServiceImp(repository, appSettingsKey, mapper);
        repository.GetAsync<Match>(Arg.Any<GetMatchesCriteria>()).Returns(new Match());

        // Act
        var result = await service.GetMatchDetail(new MatchTicketRequest());

        // Assert
        result.Should().BeEquivalentTo(new Match());

    }

    [Fact]
    public async void DeleteMatch_ReturnCorrectData()
    {
        // Arrange 
        var service = new TempVariableServiceImp(repository, appSettingsKey, mapper);
        repository.ExecuteAsync(Arg.Any<DeleteMatchCriteria>()).Returns(1);

        // Act
        var result = await service.DeleteMatch(new DeleteTempRequest());

        // Assert
        Assert.Equal(1, result);

    }
}

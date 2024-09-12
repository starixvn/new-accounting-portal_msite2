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
    private readonly IOptions<AppSettingsKey> appSettingsKey;
    private readonly IMapper mapper;

    public LogStatusServiceTests()
    {
        var settings = new AppSettingsKey
        {
            SiteDbServer = new List<string> { "ABC" }
        };
        appSettingsKey = Options.Create(settings);

        repository = Substitute.For<IDynamicRepository>();
        mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public async void GetLogStatus_ReturnCorrectData()
    {
        // Arrange
        var service = new LogStatusServiceImp(repository, appSettingsKey, mapper);
        repository.FetchAsync<LogStatus>(Arg.Any<GetLogStatusCriteria>()).Returns(new List<LogStatus>());

        // Act
        var result = await service.GetLogStatus();

        // Assert
        Assert.Equal(new List<LogStatus>(), result);
    }

    [Fact]
    public async void UpdateLogStatus_UpdateSuccess()
    {
        // Arrange
        var service = new LogStatusServiceImp(repository, appSettingsKey, mapper);
        repository.ExecuteAsync(Arg.Any<UpdateLogStatusCriteria>()).Returns(1);

        // Act
        var result = await service.UpdateLogStatus(Arg.Any<UpdateLogStatusRequest>());

        // Assert
        Assert.Equal(1, result);
    }
}
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Maintenance.DeleteTempVariable.Tests.Services;

public class SystemSettingServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly IOptions<AppSettingsKey> appSettingsKey;

    public SystemSettingServiceTests()
    {
        var settings = new AppSettingsKey
        {
            SiteDbServer = new List<string> { "ABC" }
        };
        appSettingsKey = Options.Create(settings);
        repository = Substitute.For<IDynamicRepository>();
    }

    [Fact]
    public async void GetSystemSettings_ReturnCorrectData()
    {
        // Arrange
        var service = new SystemSettingServiceImp(repository, appSettingsKey);
        repository.GetAsync<SystemSettings>(Arg.Any<GetSystemSettingCriteria>()).Returns(new SystemSettings());
        var expect = new List<SystemSettings>
        {
            new()
            {
                DbServer = "ABC"
            }
        };
        // Act
        var result = await service.GetSystemSettings();

        // Assert
        Assert.Equal(expect.Count, result.Count);
    }

    [Fact]
    public async void UpdateSystemSetting_UpdateSuccess()
    {
        // Arrange
        var service = new SystemSettingServiceImp(repository, appSettingsKey);
        repository.ExecuteAsync(Arg.Any<UpdateSystemSettingCriteria>()).Returns(1);

        // Act
        var result = await service.UpdateSystemSetting("ABC");

        // Assert
        Assert.Equal(1, result);
    }
}
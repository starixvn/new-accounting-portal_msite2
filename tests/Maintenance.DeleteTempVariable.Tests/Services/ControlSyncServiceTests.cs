using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.NAP.Client.Helpers.Web;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Maintenance.DeleteTempVariable.Tests.Services
{
    public class ControlSyncServiceTests
    {
        private readonly ICache cache;
        private readonly ILogger logger;
        private readonly INapSession session;
        private readonly IDynamicRepository repository;
        private readonly IOptions<AppSettingsKey> appSettingsKey;

        public ControlSyncServiceTests()
        {
            var settings = new AppSettingsKey();
            appSettingsKey = Options.Create(settings);
            session = Substitute.For<INapSession>();
            logger = Substitute.For<ILogger>();
            cache = Substitute.For<ICache>();
            repository = Substitute.For<IDynamicRepository>();
        }

        [Fact]
        public void UpdateStatusServiceInRedis_InputTrue_ReturnTrue()
        {
            // Arrange
            var service = new ControlSyncServicesImp(cache, logger, session, appSettingsKey, repository);

            // Act
            var result = service.UpdateStatusServiceInRedis(new Requests.UpdateStatusRequest { Status = true });

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void GetInfoService_ReturnCorrectData()
        {
            // Arrange
            var service = new ControlSyncServicesImp(cache, logger, session, appSettingsKey, repository);
            repository.FetchAsync<LastLog>(Arg.Any<GetServiceLastLogCriteria>()).Returns(new List<LastLog>());

            // Act
            var result = await service.GetInfoService();

            // Assert
            Assert.Equal(result, new List<LastLog>());
        }

    }


}

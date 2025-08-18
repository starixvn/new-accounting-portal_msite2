using AutoMapper;
using Core.Models;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria.SyncErrorLog;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests.SyncErrorLog;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Maintenance.DeleteTempVariable.Tests.Services
{
    public class SettlementErrorLogServiceTests
    {
        private readonly IDynamicRepository repository;
        private readonly IOptions<AppSettingsKey> appSettingsKey;
        private readonly IMapper mapper;
        private readonly AppSettingsKey appSettings;

        public SettlementErrorLogServiceTests()
        {
            appSettings = new AppSettingsKey
            {
                SubDbServer = new List<string> { "DB1", "DB2" }
            };
            appSettingsKey = Substitute.For<IOptions<AppSettingsKey>>();
            appSettingsKey.Value.Returns(appSettings);
            repository = Substitute.For<IDynamicRepository>();
            mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task GetSettlementErrorsLogDetails_ReturnsDetails()
        {
            // Arrange
            var details = new List<DetailSettlementErrorLog> { new DetailSettlementErrorLog { Id = 1 } };
            var criteria = new GetSettlementErrorLogDetailsCriteria();
            mapper.Map<GetSettlementErrorLogDetailsCriteria>(Arg.Any<SettlementErrorLogDetailRequest>())
                .Returns(criteria);
            repository.FetchAsync<DetailSettlementErrorLog>(criteria)
                .Returns(details);
            var service = new SettlementErrorLogServiceImp(repository, appSettingsKey, mapper);
            var request = new SettlementErrorLogDetailRequest
            {
                SessionId = 1,
                SiteDBGroupId = 2,
                DbServer = "DB1",
                Mode = "A"
            };

            // Act
            var result = await service.GetSettlementErrorsLogDetails(request);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task DeleteSettlementLockErrorsLog_ReturnsExpected()
        {
            // Arrange
            var criteria = new DeleteSettlementLockErrorLogCriteria();
            mapper.Map<DeleteSettlementLockErrorLogCriteria>(Arg.Any<DeleteSettlementLockErrorLogRequest>())
                .Returns(criteria);
            repository.ExecuteAsync(criteria)
                .Returns(1);
            var service = new SettlementErrorLogServiceImp(repository, appSettingsKey, mapper);
            var request = new DeleteSettlementLockErrorLogRequest { DbServer = "DB1" };

            // Act
            var result = await service.DeleteSettlementLockErrorsLog(request);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task DeleteSettlementProcessingErrorsLog_ReturnsExpected()
        {
            // Arrange
            var criteria = new DeleteSettlementProcessingErrorLogCriteria();
            mapper.Map<DeleteSettlementProcessingErrorLogCriteria>(Arg.Any<DeleteSettlementProcessingErrorLogRequest>())
                .Returns(criteria);
            repository.ExecuteAsync(criteria)
                .Returns(2);
            var service = new SettlementErrorLogServiceImp(repository, appSettingsKey, mapper);
            var request = new DeleteSettlementProcessingErrorLogRequest { DbServer = "DB1" };

            // Act
            var result = await service.DeleteSettlementProcessingErrorsLog(request);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task UpdateSSBSettlementErrorLog_ReturnsExpected()
        {
            // Arrange
            var criteria = new UpdateSiteDbSSBErrorLogCriteria();
            mapper.Map<UpdateSiteDbSSBErrorLogCriteria>(Arg.Any<UpdateSiteDbSSBErrorLogRequest>())
                .Returns(criteria);
            repository.ExecuteAsync(criteria)
                .Returns(3);
            var service = new SettlementErrorLogServiceImp(repository, appSettingsKey, mapper);
            var request = new UpdateSiteDbSSBErrorLogRequest { DbServer = "DB1" };

            // Act
            var result = await service.UpdateSSBSettlementErrorLog(request);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetSettlementErrorsLogs_ReturnsCombinedLogsWithCorrectTempTableNames()
        {
            // Arrange
            var processingLog = new SettlementErrorLog { DbServer = "DB1", TempTableName = null };
            var lockLog = new SettlementErrorLog { DbServer = "DB2", TempTableName = null };
            var ssbLog = new SettlementErrorLog { DbServer = "DB3", TempTableName = null };
            var request = new SettlementErrorLogRequest { MatchId = "M1", Mode = "A" };
            var service = new SettlementErrorLogServiceImp(repository, appSettingsKey, mapper);
            // Setup repository returns for each criteria
            repository.FetchAsync<SettlementErrorLog>(Arg.Any<GetSettlementProcessingErrorLogCriteria>())
                .Returns(new List<SettlementErrorLog> { processingLog });
            repository.FetchAsync<SettlementErrorLog>(Arg.Any<GetSettlementLockErrorLogCriteria>())
                .Returns(new List<SettlementErrorLog> { lockLog });
            repository.FetchAsync<SettlementErrorLog>(Arg.Any<GetSiteDbSSBErrorLogCriteria>())
                .Returns(new List<SettlementErrorLog> { ssbLog });

            // Act
            var result = await service.GetSettlementErrorsLogs(request);

            // Assert
            var expectedCount = (appSettings.SubDbServer.Count + 1) * 3;
            Assert.Equal(expectedCount, result.Count);
            Assert.Contains(result, x => x.TempTableName == "SiteDB_SettlementProcessing");
            Assert.Contains(result, x => x.TempTableName == "SiteDB_SettlementLock");
            Assert.Contains(result, x => x.TempTableName == "Settlement_SiteDB_SSBErr");
        }

        [Fact]
        public async Task GetSettlementErrorsLogs_EmptyResults_ReturnsEmptyList()
        {
            // Arrange
            var request = new SettlementErrorLogRequest { MatchId = "M1", Mode = "A" };
            var service = new SettlementErrorLogServiceImp(repository, appSettingsKey, mapper);
            repository.FetchAsync<SettlementErrorLog>(Arg.Any<GetSettlementProcessingErrorLogCriteria>())
                .Returns(new List<SettlementErrorLog>());
            repository.FetchAsync<SettlementErrorLog>(Arg.Any<GetSettlementLockErrorLogCriteria>())
                .Returns(new List<SettlementErrorLog>());
            repository.FetchAsync<SettlementErrorLog>(Arg.Any<GetSiteDbSSBErrorLogCriteria>())
                .Returns(new List<SettlementErrorLog>());

            // Act
            var result = await service.GetSettlementErrorsLogs(request);

            // Assert
            Assert.Empty(result);
        }
    }
}

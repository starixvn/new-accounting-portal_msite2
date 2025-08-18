using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Models;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using Fanex.Data.Repository;

namespace Maintenance.DeleteTempVariable.Tests.Services
{
    public class AmendmentResultServiceTests
    {
        [Fact]
        public async Task GetResults_ReturnsResultsForExpectedServers()
        {
            // Arrange
            var servers = new List<string> { "DB1", "DB2", "DBTXN" };
            var appSettings = new AppSettingsKey { SiteDbServer = servers };
            var optionsSub = Substitute.For<IOptions<AppSettingsKey>>();
            optionsSub.Value.Returns(appSettings);

            var repositorySub = Substitute.For<IDynamicRepository>();
            repositorySub.FetchAsync<AmendmentResult>(Arg.Any<Fanex.Data.Repository.ICriteria>())
                .Returns(callInfo =>
                {
                    var c = callInfo.Arg<Fanex.Data.Repository.ICriteria>() as Maintenance.DeleteTempVariable.Criteria.AmendmentResultSelCriteria;
                    return new List<AmendmentResult> {
                        new AmendmentResult { MatchId = 1, StatusId = 2, DbServer = c.DbServer }
                    };
                });

            var mapperSub = Substitute.For<IMapper>();

            var service = new AmendmentResultServiceImp(repositorySub, optionsSub, mapperSub);
            var request = new AmendmentResultRequest { StatusId = 2, MatchId = 1 };

            // Act
            var results = await service.GetResults(request);

            // Assert
            Assert.Equal(2, results.Count); // Only DB1 and DB2, not DBTXN
            Assert.All(results, r => Assert.Contains(r.DbServer, new[] { "DB1", "DB2" }));
        }

        [Fact]
        public async Task GetResults_ReturnsEmpty_WhenNoServers()
        {
            // Arrange
            var appSettings = new AppSettingsKey { SiteDbServer = new List<string>() };
            var optionsSub = Substitute.For<IOptions<AppSettingsKey>>();
            optionsSub.Value.Returns(appSettings);

            var repositorySub = Substitute.For<IDynamicRepository>();
            var mapperSub = Substitute.For<IMapper>();

            var service = new AmendmentResultServiceImp(repositorySub, optionsSub, mapperSub);
            var request = new AmendmentResultRequest { StatusId = 2, MatchId = 1 };

            // Act
            var results = await service.GetResults(request);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task GetResults_ReturnsEmpty_WhenAllServersFilteredOut()
        {
            // Arrange
            var appSettings = new AppSettingsKey { SiteDbServer = new List<string> { "DBTXN" } };
            var optionsSub = Substitute.For<IOptions<AppSettingsKey>>();
            optionsSub.Value.Returns(appSettings);

            var repositorySub = Substitute.For<IDynamicRepository>();
            var mapperSub = Substitute.For<IMapper>();

            var service = new AmendmentResultServiceImp(repositorySub, optionsSub, mapperSub);
            var request = new AmendmentResultRequest { StatusId = 2, MatchId = 1 };

            // Act
            var results = await service.GetResults(request);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task UpdateStatus_CallsRepositoryForEachItem()
        {
            // Arrange
            var appSettings = new AppSettingsKey { SiteDbServer = new List<string> { "DB1" } };
            var optionsSub = Substitute.For<IOptions<AppSettingsKey>>();
            optionsSub.Value.Returns(appSettings);

            var repositorySub = Substitute.For<IDynamicRepository>();
            var mapperSub = Substitute.For<IMapper>();
            mapperSub.Map<Maintenance.DeleteTempVariable.Criteria.AmendmentResultUpdateCriteria>(Arg.Any<AmendmentResultItem>())
                .Returns(callInfo => new Maintenance.DeleteTempVariable.Criteria.AmendmentResultUpdateCriteria());

            var service = new AmendmentResultServiceImp(repositorySub, optionsSub, mapperSub);
            var request = new AmendmentResultUpdateRequest
            {
                Results = new List<AmendmentResultItem>
                {
                    new AmendmentResultItem { MatchId = 1, StatusId = 2 },
                    new AmendmentResultItem { MatchId = 2, StatusId = 3 }
                }
            };

            // Act
            var result = await service.UpdateStatus(request);

            // Assert
            await repositorySub.Received(2).ExecuteAsync(Arg.Any<Maintenance.DeleteTempVariable.Criteria.AmendmentResultUpdateCriteria>());
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateStatus_ReturnsTrue_WhenNoItems()
        {
            // Arrange
            var appSettings = new AppSettingsKey { SiteDbServer = new List<string> { "DB1" } };
            var optionsSub = Substitute.For<IOptions<AppSettingsKey>>();
            optionsSub.Value.Returns(appSettings);

            var repositorySub = Substitute.For<IDynamicRepository>();
            var mapperSub = Substitute.For<IMapper>();

            var service = new AmendmentResultServiceImp(repositorySub, optionsSub, mapperSub);
            var request = new AmendmentResultUpdateRequest { Results = new List<AmendmentResultItem>() };

            // Act
            var result = await service.UpdateStatus(request);

            // Assert
            await repositorySub.DidNotReceive().ExecuteAsync(Arg.Any<Maintenance.DeleteTempVariable.Criteria.AmendmentResultUpdateCriteria>());
            Assert.True(result);
        }
    }
}

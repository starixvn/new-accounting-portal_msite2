using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Services;
using NSubstitute;

namespace Maintenance.DeleteTempVariable.Tests.Services
{
    public class ScaleOutSettleServiceTests
    {
        [Fact]
        public async Task GetSiteGroupId_ReturnsExpectedId()
        {
            // Arrange
            var repository = Substitute.For<IDynamicRepository>();
            var expectedId = 42;
            var dbServer = "TestServer";
            repository.GetAsync<SiteGroup>(Arg.Any<GetSiteGroupCriteria>())
                .Returns(new SiteGroup { SiteDbGroupId = expectedId });
            var service = new ScaleOutSettleServiceImp(repository);
            // Act
            var result = await service.GetSiteGroupId(dbServer);
            // Assert
            Assert.Equal(expectedId, result);
        }
    }
}

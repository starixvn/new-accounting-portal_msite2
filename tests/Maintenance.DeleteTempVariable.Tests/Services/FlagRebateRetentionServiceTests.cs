using Fanex.Caching;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Requests;
using Maintenance.DeleteTempVariable.Services;
using NSubstitute;

namespace Maintenance.DeleteTempVariable.Tests.Services
{
    public class FlagRebateRetentionServiceTests
    {
        [Fact]
        public async Task GetFlagInfo_ReturnsValueFromCache()
        {
            // Arrange
            var cache = Substitute.For<ICache>();
            cache.ContainAsync(CacheKey.RebateRetentionFlagCacheKey).Returns(true);
            cache.TryGetAsync<byte>(CacheKey.RebateRetentionFlagCacheKey).Returns((true, (byte)5));
            var service = new FlagRebateRetentionServiceImp(cache);

            // Act
            var result = await service.GetFlagInfo();

            // Assert
            Assert.Equal((byte)5, result);
        }


        [Fact]
        public async Task UpdateFlagInfo_SetsCacheAndReturnsMode()
        {
            // Arrange
            var cache = Substitute.For<ICache>();
            var service = new FlagRebateRetentionServiceImp(cache);
            var request = new UpdateFlagRebateRetentionRequest { Mode = 7 };

            // Act
            var result = await service.UpdateFlagInfo(request);

            // Assert
            await cache.Received().SetAsync(CacheKey.RebateRetentionFlagCacheKey, (byte)7, Arg.Any<CacheItemOptions>());
            Assert.Equal((byte)7, result);
        }
    }
}

using Fanex.Data.Repository;
using Maintenance.Sync.Criteria;
using Maintenance.Sync.Models;
using Maintenance.Sync.Requests;
using Maintenance.Sync.Services;
using NSubstitute;

namespace Maintenance.Sync.Tests.Services;

public class SyncMatchTransServiceTests
{
    [Fact]
    public async Task GetBatchTransId_ReturnsExpectedResults()
    {
        // Arrange
        var repository = Substitute.For<IDynamicRepository>();
        var expected = new List<SynMatchIdInfo> {
            new SynMatchIdInfo { MatchId = 1, TableName = "T", ServerName = "S" }
        };
        repository.FetchAsync<SynMatchIdInfo>(Arg.Any<SyncMatchIdSelCriteria>()).Returns(expected);
        var service = new SyncMatchTransServiceImp(repository);
        var request = new GetBatchTransIdRequest { MatchId = 1, TableName = "T", ServerName = "S" };
        // Act
        var result = await service.GetBatchTransId(request);
        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].MatchId);
        Assert.Equal("T", result[0].TableName);
        Assert.Equal("S", result[0].ServerName);
    }

    [Fact]
    public async Task UpdateBatchTransId_ReturnsError_WhenTransInfoIsNullOrEmpty()
    {
        // Arrange
        var repository = Substitute.For<IDynamicRepository>();
        repository.ExecuteAsync(Arg.Any<SyncTransIdInRangeSelCriteria>()).Returns(0);
        var service = new SyncMatchTransServiceImp(repository);
        var request = new UpdateBatchTransIdRequest { MatchId = 1, TableName = "T", ServerName = "S", FromTransId = 1, ToTransId = 2, ToServerNames = new string[] { "S1" } };
        // Act
        var result = await service.UpdateBatchTransId(request);
        // Assert
        Assert.Equal(101, result.ErrCode);
        Assert.Contains("Empty", result.ErrMsg);
    }

    [Fact]
    public async Task UpdateBatchTransId_ReturnsError_WhenAnySiteUpdateFails()
    {
        // Arrange
        var repository = Substitute.For<IDynamicRepository>();
        var transInfo = new byte[] { 1, 2, 3 };
        repository.ExecuteAsync(Arg.Any<SyncTransIdInRangeSelCriteria>()).Returns(x => { ((SyncTransIdInRangeSelCriteria)x[0]).TransInfoJson = transInfo; return 0; });
        repository.ExecuteAsync(Arg.Any<SyncTransIdInRangeUpdCriteria>()).Returns(x => { ((SyncTransIdInRangeUpdCriteria)x[0]).ErrCode = 500; ((SyncTransIdInRangeUpdCriteria)x[0]).ErrMsg = "Failed"; return 0; });
        var service = new SyncMatchTransServiceImp(repository);
        var request = new UpdateBatchTransIdRequest { MatchId = 1, TableName = "T", ServerName = "S", FromTransId = 1, ToTransId = 2, ToServerNames = new string[] { "S1", "S2" } };
        // Act
        var result = await service.UpdateBatchTransId(request);
        // Assert
        Assert.Equal(500, result.ErrCode);
        Assert.Contains("Failed", result.ErrMsg);
    }

}

using Maintenance.Core.Models;
using Maintenance.Sync.Criteria;
using Maintenance.Sync.Models;
using Maintenance.Sync.Requests;

namespace Maintenance.Sync.Services;

public interface SyncMatchTransService
{
    Task<List<SynMatchIdInfo>> GetBatchTransId(GetBatchTransIdRequest request);

    Task<Result> UpdateBatchTransId(UpdateBatchTransIdRequest request);
}

public class SyncMatchTransServiceImp : SyncMatchTransService
{
    private readonly IDynamicRepository repository;

    public SyncMatchTransServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<SynMatchIdInfo>> GetBatchTransId(GetBatchTransIdRequest request)
    {
        var criteria = new SyncMatchIdSelCriteria
        {
            MatchId = request.MatchId,
            TableName = request.TableName,
            DbServer = request.ServerName,
        };

        var results = (await repository.FetchAsync<SynMatchIdInfo>(criteria)).ToList();

        results.ForEach(result =>
        {
            result.MatchId = request.MatchId;
            result.TableName = request.TableName;
            result.ServerName = request.ServerName;
        });

        return results;
    }

    public async Task<Result> UpdateBatchTransId(UpdateBatchTransIdRequest request)
    {
        var transInfo = await GetTransInfo(request);

        if (transInfo == null || !transInfo.Any()) return new Result();

        var result = await UpdateTransInfoAllSite(request, transInfo);

        return result;
    }

    private async Task<byte[]> GetTransInfo(UpdateBatchTransIdRequest request)
    {
        var selCriteria = new SyncTransIdInRangeSelCriteria
        {
            MatchId = request.MatchId,
            FromTransId = request.FromTransId,
            ToTransId = request.ToTransId,
            TableName = request.TableName,
            ServerName = request.ServerName,
        };

        await repository.ExecuteAsync(selCriteria);

        return selCriteria.TransInfoJson;
    }

    private async Task<Result> UpdateTransInfoAllSite(UpdateBatchTransIdRequest request, byte[] transInfo)
    {
        var updCriteria = new SyncTransIdInRangeUpdCriteria
        {
            TableName = request.TableName,
            TransInfoJson = transInfo,
        };
        var tasks = request.ToServerNames.Select(s => UpdateBatchTransIdOneSite(s, updCriteria));

        var results = await Task.WhenAll(tasks);

        return new Result();
    }

    private async Task<Result> UpdateBatchTransIdOneSite(string s, SyncTransIdInRangeUpdCriteria criteria)
    {
        criteria.ServerName = s;

        await repository.ExecuteAsync(criteria);

        return new Result
        {
            ErrCode = criteria.ErrCode,
            ErrMsg = criteria.ErrMsg
        };
    }
}
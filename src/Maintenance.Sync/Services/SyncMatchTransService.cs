using Maintenance.Sync.Criteria;
using Maintenance.Sync.Models;
using Maintenance.Sync.Requests;

namespace Maintenance.Sync.Services;

using Core.Models;

public interface SyncMatchTransService
{
    Task<List<SynMatchIdInfo>> GetBatchTransId(GetBatchTransIdRequest request);

    Task<Result> UpdateBatchTransId(UpdateBatchTransIdRequest request);
}

public class SyncMatchTransServiceImp(IDynamicRepository repository) : SyncMatchTransService
{
    public async Task<List<SynMatchIdInfo>> GetBatchTransId(GetBatchTransIdRequest request)
    {
        var criteria = new SyncMatchIdSelCriteria
        {
            MatchId = request.MatchId,
            TableName = request.TableName,
            DbServer = request.ServerName
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

        if (transInfo == null || !transInfo.Any())
        {
            return new Result
            {
                ErrCode = 101,
                ErrMsg = "TransInfoJson get from NAP_Set_SiteDB_SyncTransIdInRange_Sel is Empty"
            };
        }

        var results = await UpdateTransInfoAllSite(request, transInfo);

        var errorResults = results.Where(x => !x.IsSuccess);

        if (errorResults.Any())
        {
            return new Result
            {
                ErrCode = errorResults.First().ErrCode,
                ErrMsg = string.Join('|', errorResults.Select(x => x.ErrMsg))
            };
        }

        return new Result();
    }

    private async Task<byte[]> GetTransInfo(UpdateBatchTransIdRequest request)
    {
        var selCriteria = new SyncTransIdInRangeSelCriteria
        {
            MatchId = request.MatchId,
            FromTransId = request.FromTransId,
            ToTransId = request.ToTransId,
            TableName = request.TableName,
            ServerName = request.ServerName
        };

        await repository.ExecuteAsync(selCriteria);

        return selCriteria.TransInfoJson;
    }

    private async Task<List<Result>> UpdateTransInfoAllSite(UpdateBatchTransIdRequest request, byte[] transInfo)
    {
        var updCriteria = new SyncTransIdInRangeUpdCriteria
        {
            TableName = request.TableName,
            TransInfoJson = transInfo
        };
        var tasks = request.ToServerNames.Select(s => UpdateBatchTransIdOneSite(s, updCriteria));

        var results = await Task.WhenAll(tasks);

        return results.ToList();
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
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;

namespace Maintenance.DeleteTempVariable.Services;

public interface SettlementErrorLogService
{
    Task<SettlementErrorLog> GetLogDetails();
    Task<List<SettlementErrorLog>> GetLogs(SettlementErrorLogRequest request);
}

public class SettlementErrorLogServiceImp : SettlementErrorLogService
{
    private readonly IDynamicRepository repository;

    public SettlementErrorLogServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<SettlementErrorLog>> GetLogs(SettlementErrorLogRequest request)
    {
        return new List<SettlementErrorLog>();
        //return (await repository.FetchAsync<SettlementErrorLog>(new GetSyncErrorLogCriteria())).ToList();
    }

    public async Task<SettlementErrorLog> GetLogDetails()
    {
        return new SettlementErrorLog();
    }
}

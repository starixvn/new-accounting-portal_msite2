using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

public interface SyncTransIdDirectlyService
{
    DbInfo GetDbInfo();
    Task<Result> UpdateTransId(UpdateTransIdsRequest request);
}

public class SyncTransIdDirectlyServiceImp : SyncTransIdDirectlyService
{
    private readonly AppSettingsKey appSettings;
    private readonly IDynamicRepository repository;
    private readonly IMapper mapper;

    public SyncTransIdDirectlyServiceImp(IOptions<AppSettingsKey> appSettings, IDynamicRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.appSettings = appSettings.Value;
    }

    public DbInfo GetDbInfo()
    {
        return new DbInfo
        {
            Tables = new List<string> { "bettrans ", "bettransm" },
            DbSites = appSettings.SiteDbServer.Where(s => !s.Equals(DataBaseServers.DBCentral)).ToList(),
        };
    }

    public async Task<Result> UpdateTransId(UpdateTransIdsRequest request)
    {
        var criteria = mapper.Map<UpdateTransIdsCriteria>(request);

        await repository.ExecuteAsync(criteria);

        return new Result
        {
            ErrCode = criteria.ErrCode,
            ErrMsg = criteria.ErrMsg,
        };
    }
}

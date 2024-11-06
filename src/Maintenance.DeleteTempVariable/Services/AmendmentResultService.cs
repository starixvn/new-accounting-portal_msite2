using AutoMapper;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Constants;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

using Core.Models;

public interface AmendmentResultService
{
    Task<bool> UpdateStatus(AmendmentResultUpdateRequest request);

    Task<List<AmendmentResult>> GetResults(AmendmentResultRequest request);
}

public class AmendmentResultServiceImp(
    IDynamicRepository repository,
    IOptions<AppSettingsKey> options,
    IMapper mapper)
    : AmendmentResultService
{
    private readonly AppSettingsKey appSettingsKey = options.Value;

    public async Task<List<AmendmentResult>> GetResults(AmendmentResultRequest request)
    {
        var results = new List<AmendmentResult>();
        foreach (var server in appSettingsKey.SiteDbServer.Where(s => s != "DBTXN"))
        {
            var criteria = new AmendmentResultSelCriteria
            {
                DbServer = server,
                StatusId = request.StatusId,
                MatchId = request.MatchId
            };

            var list = (await repository.FetchAsync<AmendmentResult>(criteria)).ToList();
            list.ForEach(i => i.DbServer = server);

            results.AddRange(list);
        }

        return results;
    }

    public async Task<bool> UpdateStatus(AmendmentResultUpdateRequest request)
    {
        foreach (var item in request.Results)
        {
            var criteria = Create(item);

            await repository.ExecuteAsync(criteria);
        }

        return true;
    }

    private AmendmentResultUpdateCriteria Create(AmendmentResultItem item)
    {
        var criteria = mapper.Map<AmendmentResultUpdateCriteria>(item);

        criteria.StatusId = item.StatusId switch
        {
            (short)AmendmentStatus.FailSettle => (short)AmendmentStatus.Complete,
            (short)AmendmentStatus.TimeOut => (short)AmendmentStatus.Running,
            _ => 0
        };

        return criteria;
    }
}
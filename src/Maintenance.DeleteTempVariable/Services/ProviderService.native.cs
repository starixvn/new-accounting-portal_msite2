using Core.Models;
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Maintenance.DeleteTempVariable.Requests;

namespace Maintenance.DeleteTempVariable.Services;

public class ProviderService(IDynamicRepository repository)
{
    public async Task<List<Provider>> GetProvidersBySport(int sportType)
    {
        var criteria = new ProviderInfoSelCriteria
        {
            SportType = sportType
        };
        var result = await repository.FetchAsync<Provider>(criteria);
        var providers = result.OrderBy(x => x.SourceId).ToList();
        providers.ForEach(x => x.SportType = sportType);

        return providers;
    }

    public async Task<List<Provider>> GetProviders()
    {
        var criteria = new GetAllProvidersCriteria();
        var result = await repository.FetchAsync<Provider>(criteria);
        return [.. result.OrderBy(x => x.SourceId)];
    }

    public async Task<Result> MappingProvider(ProviderMappingRequest request)
    {
        var criteria = new ProviderGameTypeMappingCriteria
        {
            SourceId = request.SourceId,
            SportType = request.SportType,
            GameTypes = request.GameType,
            IsActive = request.IsActive
        };

        await repository.ExecuteAsync(criteria);

        return new Result
        {
            ErrCode = criteria.ErrCode,
            ErrMsg = criteria.ErrMsg
        };
    }
}

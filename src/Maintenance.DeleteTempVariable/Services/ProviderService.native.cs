using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;

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
        return [.. result.OrderBy(x => x.SourceId)];
    }
}

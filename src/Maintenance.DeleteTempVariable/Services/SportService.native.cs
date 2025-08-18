using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;

namespace Maintenance.DeleteTempVariable.Services;

public class SportService(IDynamicRepository repository)
{
    public async Task<List<Sport>> GetSports()
    {
        var criteria = new GetAllSportCriteria();

        var result = await repository.FetchAsync<Sport>(criteria);

        return [.. result.OrderBy(x => x.SportName)];
    }
}

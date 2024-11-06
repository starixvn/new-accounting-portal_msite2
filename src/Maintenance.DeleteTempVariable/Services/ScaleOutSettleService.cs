namespace Maintenance.DeleteTempVariable.Services;

using Criteria;
using Fanex.Data.Repository;
using Model;

public interface ScaleOutSettleService
{
    Task<int> GetSiteGroupId(string dbServer);
}

public class ScaleOutSettleServiceImp(IDynamicRepository repository) : ScaleOutSettleService
{
    public async Task<int> GetSiteGroupId(string dbServer)
    {
        var criteria = new GetSiteGroupCriteria { DbServer = dbServer };
        var site = await repository.GetAsync<SiteGroup>(criteria);
        return site.SiteDbGroupId;
    }
}
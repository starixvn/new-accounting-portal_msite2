using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;

namespace Maintenance.DeleteTempVariable.Services;

public interface SystemSettingService
{
    Task<string> GetSystemSettings();

    Task<int> UpdateSystemSetting();
}

public class SystemSettingServiceImp : SystemSettingService
{
    private readonly IDynamicRepository repository;

    public SystemSettingServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<string> GetSystemSettings()
    {
        return await repository.GetAsync<string>(new GetSystemSettingCriteria());
    }

    public async Task<int> UpdateSystemSetting()
    {
        return await repository.ExecuteAsync(new UpdateSystemSettingCriteria());
    }
}
using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Criteria;
using Maintenance.DeleteTempVariable.Model;
using Microsoft.Extensions.Options;

namespace Maintenance.DeleteTempVariable.Services;

using Core.Models;

public interface SystemSettingService
{
    Task<List<SystemSettings>> GetSystemSettings();

    Task<int> UpdateSystemSetting(string dbServer);
}

public class SystemSettingServiceImp(IDynamicRepository repository,
        IOptions<AppSettingsKey> appSettingsKey)
    : SystemSettingService
{
    private readonly AppSettingsKey appSettingsKey = appSettingsKey.Value;

    public async Task<List<SystemSettings>> GetSystemSettings()
    {
        var list = new List<SystemSettings>();

        foreach (var server in appSettingsKey.SiteDbServer)
        {
            var criteria = new GetSystemSettingCriteria
            {
                DbServer = server
            };
            var setting = await repository.GetAsync<SystemSettings>(criteria);
            if (setting != null)
            {
                setting.DbServer = server;
                list.Add(setting);
            }
        }

        return list;
    }

    public async Task<int> UpdateSystemSetting(string dbServer)
    {
        return await repository.ExecuteAsync(new UpdateSystemSettingCriteria { DbServer = dbServer });
    }
}
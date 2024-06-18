using Maintenance.Sync.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Maintenance.Sync;

public static class Injector
{
    public static IServiceCollection AddSyncServices(this IServiceCollection services)
    {
        services.AddScoped<SyncMatchTransService, SyncMatchTransServiceImp>();
        services.AddScoped<SyncTransDirectlyService, SyncTransDirectlyServiceImp>();
        return services;
    }
}
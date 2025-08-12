using Fanex.Data.Repository;
using Maintenance.DeleteTempVariable.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Maintenance.Apps;

using Fanex.Caching;
using Fanex.Caching.Redis;
using Fanex.RemotingRef;
using Maintenance.DeleteTempVariable;
using SpecialLeagueSetting.Services;
using Sync;

public static class ServiceCollectionExtensions
{
    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

        services.AddMvc();
    }

    public static void AddControllerAuthorization(this IServiceCollection services)
    {
        services.AddControllers(config =>
        {
            var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        });
        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }
        );
    }

    public static void AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = configuration.GetSection("AuthSettings:LoginPath").Value;
                        options.LogoutPath = configuration.GetSection("AuthSettings:LogoutPath").Value;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:CookieTimeOut").Value));
                        options.Cookie = new CookieBuilder
                        {
                            Name = "Fanex.mSite2.Cookie",
                            SecurePolicy = CookieSecurePolicy.SameAsRequest
                        };
                        options.Events.OnSigningOut = context =>
                        {
                            context.Response.Redirect(options.LoginPath);
                            return Task.CompletedTask;
                        };
                    });
    }

    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IDynamicRepository, DynamicRepository>();
    }

    public static void AddFanexServices(this IServiceCollection services, IConfiguration configuration)
    {
        RefDataTypeContainer
            .ConnectTo(new Uri(configuration.GetSection("Fanex.RemotingRef.Service.Url").Value))
            .FromApplication("Settlement");

        services.AddSingleton<IRemotingRefService, RemotingRefService>();
    }

    public static void AddInternalServices(this IServiceCollection services)
    {
        services.AddTransient<ParlayLockingService, ParlayLockingServiceImp>();
        services.AddTransient<SettlementLockService, SettlementLockServiceImp>();
        services.AddTransient<SystemSettingService, SystemSettingServiceImp>();
        services.AddTransient<TempVariableService, TempVariableServiceImp>();
        services.AddTransient<LogStatusService, LogStatusServiceImp>();
        services.AddTransient<ControlSyncService, ControlSyncServicesImp>();
        services.AddTransient<SettlementErrorLogService, SettlementErrorLogServiceImp>();
        services.AddTransient<ScaleOutSettleService, ScaleOutSettleServiceImp>();
        services.AddTransient<FlagBroadcastService, FlagBroadcastServiceImp>();
        services.AddTransient<AmendmentResultService, AmendmentResultServiceImp>();
        services.AddTransient<LeagueSettingService, LeagueSettingServiceImp>();
        services.AddTransient<FlagRebateRetentionService, FlagRebateRetentionServiceImp>();
        services.AddTransient<SportService>();
        services.AddTransient<ProviderService>();
        services.AddSyncServices();
    }

    public static void AddRedisSession(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSession(options =>
        {
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:SessionTimeOut").Value));
            options.Cookie.Name = "Fanex.mSite2.Session";
        });

        var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
        var redisMultiplexer = ConnectionMultiplexer.Connect(redisConfig);

        services.AddDataProtection()
            .PersistKeysToStackExchangeRedis(redisMultiplexer, "Fanex.nSite2-ProtectionKeys");

        services.AddSingleton(redisMultiplexer);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig;
            options.InstanceName = "Fanex.mSite2.Session-";
        });
    }

    public static void AddConfigRedisCache(this IServiceCollection service, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection("AuthSettings:RedisConfig").Value;
        var applicationName = configuration.GetSection("AuthSettings:ChannelPrefix").Value;
        service.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect($"{redisConfig},ChannelPrefix={applicationName}"));
        service.AddSingleton<ICache>(new RedisCache(new RedisCacheOptions
        {
            ConnectionString = redisConfig,
            ApplicationName = applicationName,
        }));
    }
}
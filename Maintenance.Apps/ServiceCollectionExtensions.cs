
using AutoMapper;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;
using Fanex.Logging.Extensions.Database;
using Fanex.Logging.Sentry;
using Fanex.NAP.Client.AspNetCore.Configuration;
using Fanex.NAP.Client.AspNetCore.Session;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using StackExchange.Redis;
using System.Reflection;

namespace Maintenance.Apps;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
        });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddMvc();

        return services;
    }

    public static IServiceCollection AddControllerAuthorization(this IServiceCollection services)
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

        return services;
    }

    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = configuration.GetSection("AuthSettings:LoginPath").Value;
                        options.LogoutPath = configuration.GetSection("AuthSettings:LogoutPath").Value;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:CookieTimeOut").Value));
                        options.Cookie = new CookieBuilder
                        {
                            Name = "Fanex.OWP.Cookie",
                            SecurePolicy = CookieSecurePolicy.SameAsRequest
                        };
                        options.Events.OnSigningOut = context =>
                        {
                            context.Response.Redirect(options.LoginPath);
                            return Task.CompletedTask;
                        };
                    });

        return services;
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IDynamicRepository, DynamicRepository>();
        return services;
    }


    public static IServiceCollection AddFanexServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connections = configuration.GetSection("ConnectionStrings")
                               .GetChildren()
                               .AsEnumerable()
                               .ToDictionary(
                                        x => x.GetValue<string>("Name"),
                                        y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName"))
                                );
        var storedProcedurePath = configuration.GetValue<string>("StoredProcedurePath");

        DbSettingProviderManager
          .StartNewSession()
          .Use(connections)
          .WithSqlServer()
              .UseDefaultDbSettingProvider(resourcePath: storedProcedurePath)
          .Run();

        LogManager
        .SetDefaultLogCategory(configuration.GetSection("LogCategory").Value)
        .Use(new List<ILogEngine>
        {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = configuration.GetSection("SentryEngineOptions:Dsn").Value
                })
        });

        ExceptionHandler
            .UseConfig(new LoggingConfiguration())
            .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

        return services;
    }

    public static IServiceCollection AddInternalServices(this IServiceCollection services)
    {

        return services;
    }

    public static IServiceCollection AddMediator(this IServiceCollection services)
    {

        return services;
    }

    public static IServiceCollection AddNapServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<Fanex.Logging.ILogger, Logger>();
        services.AddSingleton(new UMClientConfiguration().Initialize(configuration));

        services.AddSingleton<INapSettings>(new NapSettings(configuration));
        services.AddSingleton<INapSession, NapSession>();

        services.AddSingleton<IGroupService, GroupService>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<ISecurityService, SecurityService>();
        services.AddSingleton<ISiteService, SiteService>();

        services.AddSingleton<IMaintenanceService, MaintenanceService>();
        services.AddSingleton<IUMCacheService, UMCacheService>();
        services.AddSingleton<IUMRestClient, UMRestClient>();
        services.AddSingleton<IHttpContextHelper, HttpContextHelper>();

        return services;
    }

    public static IServiceCollection AddRedisSession(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSession(options =>
        {
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(int.Parse(configuration.GetSection("AuthSettings:SessionTimeOut").Value));
            options.Cookie.Name = "Fanex.OWP.Session";
        });

        return services;
    }
}
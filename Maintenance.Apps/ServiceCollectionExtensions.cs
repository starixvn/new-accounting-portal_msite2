using AutoMapper;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Client.Services;
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
using Fanex.Logging.Sentry.RavenClient;
using Fanex.NAP.Client.Session;
using Fanex.RemotingRef;
using Fanex.Settlement.ScaleOut.Services;
using Maintenance.SpecialLeagueSetting.Services;
using Maintenance.Sync;
using SharpRaven;

public static class ServiceCollectionExtensions
{
	public static void AddAutoMapper(this IServiceCollection services)
	{
		var mapperConfig = new MapperConfiguration(mc =>
		{
			mc.AddProfile(new DeleteTempVariable.AutoMapperProfile());
		});

		var mapper = mapperConfig.CreateMapper();
		services.AddSingleton(mapper);

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

	public static IServiceCollection AddFanexServices(this IServiceCollection services, IConfiguration configuration)
	{
		LogManager
		.SetDefaultLogCategory(configuration.GetSection("LogCategory").Value)
		.Use(new List<ILogEngine>
		{
				new SentryLogging(new SentryEngineOptions
				{
					Dsn = new Dsn(configuration.GetSection("SentryEngineOptions:Dsn").Value)
				})
		});

		ExceptionHandler
			.UseConfig(new LoggingConfiguration())
			.UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

		services.AddSingleton(Logger.Log);

		RefDataTypeContainer
			.ConnectTo(new Uri(configuration.GetSection("Fanex.RemotingRef.Service.Url").Value))
			.FromApplication("Settlement");

		services.AddSingleton<IRemotingRefService, RemotingRefService>();


		return services;
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
		services.AddTransient<GeneralService, GeneralServiceImp>();
		services.AddTransient<ScaleOutSettleService, ScaleOutSettleServiceImp>();
		services.AddTransient<FlagBroadcastService, FlagBroadcastServiceImp>();
		services.AddTransient<AmendmentResultService, AmendmentResultServiceImp>();
		services.AddTransient<LeagueSettingService, LeagueSettingServiceImp>();

		services.AddSyncServices();
	}

	public static void AddNapServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddTransient<Fanex.Logging.ILogger, Logger>();
		services.AddSingleton<INapSettings>(new NapSettings(configuration));
		services.AddSingleton<INapSession, NapSession>();

		services.AddSingleton<IGroupService, GroupService>();
		services.AddSingleton<IUserService, UserService>();
		services.AddSingleton<ISecurityService, SecurityService>();
		services.AddSingleton<ISiteService, SiteService>();
	}

	public static void AddOnebookUM(this IServiceCollection services, IConfiguration configuration)
	{
		services.RegisterServiceCollection(configuration);
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
			ApplicationName = applicationName
		}));
		service.AddSingleton<ICacheService>(new CacheService());
	}
}
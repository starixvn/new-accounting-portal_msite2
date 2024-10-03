using Fanex.Data.SqlServer;
using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.SignIn.Attribute;
using FluentValidation;
using Maintenance.Apps;
using Maintenance.Apps.Apis;
using Maintenance.Core.Models;
using Maintenance.SpecialLeagueSetting.Request;
using Microsoft.AspNetCore.Mvc;
using ONEbook.UM.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	ApplicationName = typeof(Program).Assembly.FullName,
	ContentRootPath = Directory.GetCurrentDirectory()
});

var services = builder.Services;
var configuration = builder.Configuration;

configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
services.Configure<AppSettingsKey>(configuration.GetSection(AppSettingsKey.MultiDbServer));
services.Configure<AppSettingsKey>(configuration);

// Add services to the container.
services.AddSession();
services.AddRazorPages();
services.AddControllersWithViews(options =>
{
	options.Filters.Add(new ServiceFilterAttribute(typeof(CheckUMFilterAttribute)));
	options.Filters.Add(new SessionAuthorizeAttribute());
}).AddNewtonsoftJson();

services.AddControllerAuthorization();
services.AddCookieAuthentication(configuration);
services.AddRedisSession(configuration);
services.AddConfigRedisCache(configuration);

services.AddNapServices(configuration);
services.AddFanexServices(configuration);
services.AddFanexSql(configuration);
services.AddOnebookUM(configuration);
services.AddCoreServices();
services.AddAutoMapper();
services.AddInternalServices();
services.AddControllersWithViews(options =>
{
	options.Filters.Add(new ServiceFilterAttribute(typeof(CheckUMFilterAttribute)));
	options.Filters.Add(new SessionAuthorizeAttribute());
}).AddNewtonsoftJson();

services.AddValidatorsFromAssemblyContaining<UpdateSettingRequestValidator>();


// Configure app
var app = builder.Build();
app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseCookiePolicy();
app.UseSession();
app.UseMiddleware<ErrorHandler>();
app.UseRouting();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapSyncTransApi();
app.MapSpecialLeagueSettingApi();

app.MapControllerRoute(
	"default",
	"{controller=Home}/{action=Index}/{id?}");

NapClientManager
	.UseConfig(app.Services.GetService<INapSettings>())
	.UseSession(app.Services.GetService<INapSession>());

app.Run();
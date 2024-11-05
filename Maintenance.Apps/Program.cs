using Core.Models;
using Fanex.Data;
using Fanex.NAP.Client.Attributes;
using FluentValidation;
using Maintenance.Apps;
using Maintenance.Apps.Apis;
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

services.AddFanexServices(configuration);
services.AddFanexData(configuration);
services.AddNapClient(configuration);
services.AddOneBookUm(configuration);
services.AddCoreServices();
services.AddAutoMapper();
services.AddInternalServices();

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

await app.RunAsync();
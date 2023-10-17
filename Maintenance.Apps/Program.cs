using Fanex.NAP.Client;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Signin.AspNetCore.Attribute;
using Maintenance.Apps;
using Maintenance.Apps.App_Start;
using Maintenance.DeleteTempVariable.Model;
using Microsoft.AspNetCore.Mvc;
using ONEbook.UM.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory()
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<AppSettingsKey>(builder.Configuration.GetSection(AppSettingsKey.MultiDbServer));
builder.Services.Configure<AppSettingsKey>(builder.Configuration);

// Add services to the container.
builder.Services.AddSession();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ServiceFilterAttribute(typeof(CheckUMFilterAttribute)));
    options.Filters.Add(new SessionAuthorizeAttribute());
}).AddNewtonsoftJson();

builder.Services.AddControllerAuthorization();
builder.Services.AddCookieAuthentication(builder.Configuration);
builder.Services.AddRedisSession(builder.Configuration);
builder.Services.AddConfigRedisCache(builder.Configuration);

builder.Services.AddNapServices(builder.Configuration);
builder.Services.AddFanexServices(builder.Configuration);
builder.Services.AddOnebookUM(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddAutoMapper();
builder.Services.AddInternalServices();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ServiceFilterAttribute(typeof(CheckUMFilterAttribute)));
    options.Filters.Add(new SessionAuthorizeAttribute());
}).AddNewtonsoftJson();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

NapClientManager
    .UseConfig(app.Services.GetService<INapSettings>())
    .UseSession(app.Services.GetService<INapSession>());

#if DEBUG
app.Run();
#endif

await app.RunAsync("http://0.0.0.0:5000");
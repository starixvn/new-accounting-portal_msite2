namespace Maintenance.Apps;

using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class ErrorHandler(RequestDelegate next, ILogger logger, IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleException(context, exception);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        var logMessage = new LogMessage(exception.Message)
                    .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                    .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                    .ToString();
        await logger.ErrorAsync(logMessage);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsync("InternalServerError");

        var env = Configuration.GetSection("Fanex.NAP.Client:Environment").Value;

        if (!env.Equals("LIVE", StringComparison.CurrentCultureIgnoreCase))
        {
            await context.Response.WriteAsync(Environment.NewLine);
            await context.Response.WriteAsync(exception.StackTrace ?? string.Empty);
        }
    }
}
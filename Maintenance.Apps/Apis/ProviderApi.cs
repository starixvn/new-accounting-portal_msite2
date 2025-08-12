using Maintenance.DeleteTempVariable.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Apps.Apis;

public static class ProviderApi
{
    public static IEndpointRouteBuilder MapProviderApi(this IEndpointRouteBuilder app)
    {
        var goup = app.MapGroup("api/delete-temp-variable");

        goup.MapGet("/get-providers/{sportType}", async (int sportType, [FromServices] ProviderService service) =>
        {
            var result = await service.GetProvidersBySport(sportType);
            return TypedResults.Ok(result);
        });
        return app;
    }
}

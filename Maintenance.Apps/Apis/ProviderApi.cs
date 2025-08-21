using Maintenance.DeleteTempVariable.Requests;
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

        goup.MapGet("/get-all-providers", async ([FromServices] ProviderService service) =>
        {
            var result = await service.GetProviders();
            return TypedResults.Ok(result);
        });

        goup.MapPost("/provider-mapping", async (ProviderMappingRequest request, [FromServices] ProviderService service) =>
        {
            var result = await service.MappingProvider(request);

            return TypedResults.Ok(result);
        });


        return app;
    }
}

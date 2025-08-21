using Maintenance.DeleteTempVariable.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Apps.Apis;

public static class SportApi
{
    public static IEndpointRouteBuilder MapSportApi(this IEndpointRouteBuilder app)
    {
        var goup = app.MapGroup("api/delete-temp-variable");

        goup.MapGet("/sport", async ([FromServices] SportService service) =>
        {
            var result = await service.GetSports();
            return TypedResults.Ok(result);
        });
        return app;
    }
}

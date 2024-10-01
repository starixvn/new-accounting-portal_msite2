using Maintenance.SpecialLeagueSetting.Request;
using Maintenance.SpecialLeagueSetting.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Apps.Apis;

public static class SpecialLeagueSettingApi
{
	public static IEndpointRouteBuilder MapSpecialLeagueSettingApi(this IEndpointRouteBuilder app)
	{
		var api = app.MapGroup("api/delete-temp-variable");

		api.MapPost("/league-setting", async (
			[FromBody] GetLeagueSetting request,
			[FromServices] LeagueSettingService service) => TypedResults.Ok(await service.GetLeagueSetting(request)));

		api.MapPost("/update-league-setting", async (
			[FromBody] UpdateSettingRequest request,
			[FromServices] LeagueSettingService service) => TypedResults.Ok(await service.UpdateLeagueSetting(request)));

		return app;
	}
}
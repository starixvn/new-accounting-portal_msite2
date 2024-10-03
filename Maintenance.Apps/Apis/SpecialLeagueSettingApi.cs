using FluentValidation;
using Maintenance.SpecialLeagueSetting.Request;
using Maintenance.SpecialLeagueSetting.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.Apps.Apis;

public static class SpecialLeagueSettingApi
{
	public static IEndpointRouteBuilder MapSpecialLeagueSettingApi(this IEndpointRouteBuilder app)
	{
		var api = app.MapGroup("api/delete-temp-variable");

		api.MapPost("/league-setting", async ([FromBody] GetLeagueSetting request, [FromServices] LeagueSettingService service) => TypedResults.Ok(await service.GetLeagueSetting(request)));

		api.MapPost("/update-league-setting", UpdateLeagueSetting);

		api.MapPost("/remove-league-setting", RemoveSettingSetting);

		return app;
	}

	private static async Task<Results<Ok<bool>, ValidationProblem>> RemoveSettingSetting(
			[FromBody] RemoveSettingRequest request,
			[FromServices] LeagueSettingService service,
			[FromServices] IValidator<RemoveSettingRequest> validator)
	{
		var validationResult = validator.Validate(request);
		if (!validationResult.IsValid)
		{
			return TypedResults.ValidationProblem(validationResult.ToDictionary());
		}
		return TypedResults.Ok(await service.RemoveLeagueSetting(request));
	}

	private static async Task<Results<Ok<bool>, ValidationProblem>> UpdateLeagueSetting(
			[FromBody] UpdateSettingRequest request,
			[FromServices] IValidator<UpdateSettingRequest> validator,
			[FromServices] LeagueSettingService service)
	{
		var validationResult = validator.Validate(request);
		if (!validationResult.IsValid)
		{
			return TypedResults.ValidationProblem(validationResult.ToDictionary());
		}

		return TypedResults.Ok(await service.UpdateLeagueSetting(request));

	}
}

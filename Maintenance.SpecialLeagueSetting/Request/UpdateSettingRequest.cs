using FluentValidation;
using Maintenance.SpecialLeagueSetting.Constants;

namespace Maintenance.SpecialLeagueSetting.Request;

public class UpdateSettingRequest
{
	public int LeagueId { get; set; }
	public byte? AutoType { get; set; }
}


public class UpdateSettingRequestValidator : AbstractValidator<UpdateSettingRequest>
{
	public UpdateSettingRequestValidator()
	{
		RuleFor(x => x.LeagueId)
			.GreaterThan(0)
			.WithMessage("LeagueId must be greater than 0");

		byte[] supportedAutoTypes = [AutoTypeSetting.DirectSabaSoccer, AutoTypeSetting.FantasySoccer, AutoTypeSetting.FantasyBasketball];

		RuleFor(x => x.AutoType)
			.Must(x => supportedAutoTypes.Contains(x.Value))
			.When(x => x.AutoType.HasValue)
			 .WithMessage("Please only use: " + string.Join(", ", supportedAutoTypes));
	}
}

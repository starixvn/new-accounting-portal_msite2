using FluentValidation;
using Maintenance.SpecialLeagueSetting.Constants;

namespace Maintenance.SpecialLeagueSetting.Request;

public class UpdateSettingRequest
{
	public int LeagueId { get; set; }
	public byte AutoType { get; set; }
	public byte SportType { get; set; }
}


public class UpdateSettingRequestValidator : AbstractValidator<UpdateSettingRequest>
{
	public UpdateSettingRequestValidator()
	{
		RuleFor(x => x.LeagueId).NotEmpty().WithMessage("LeagueId is required");

		byte[] supportedAutoTypes = [AutoTypeSetting.DirectSabaSoccer, AutoTypeSetting.FantasySoccer, AutoTypeSetting.FantasyBasketball];
		RuleFor(x => x.AutoType)
			.Must(x => supportedAutoTypes.Contains(x))
			.WithMessage("Please only use: " + string.Join(", ", supportedAutoTypes));
	}
}

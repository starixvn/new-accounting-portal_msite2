using FluentValidation;
using Maintenance.SpecialLeagueSetting.Constants;

namespace Maintenance.SpecialLeagueSetting.Request;

public class GetLeagueSetting
{
	public int? LeagueId { get; set; }
	public byte? AutoType { get; set; }
}

public class GetLeagueSettingValidator : AbstractValidator<GetLeagueSetting>
{
	public GetLeagueSettingValidator()
	{
		byte[] supportedAutoTypes = [AutoTypeSetting.DirectSabaSoccer, AutoTypeSetting.FantasySoccer, AutoTypeSetting.FantasyBasketball];

		RuleFor(x => x.AutoType)
			.Must(x => supportedAutoTypes.Contains(x.Value))
			.When(x => x.AutoType.HasValue)
			 .WithMessage("Please only use: " + string.Join(", ", supportedAutoTypes));
	}
}

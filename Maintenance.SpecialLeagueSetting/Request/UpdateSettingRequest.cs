using FluentValidation;
using Maintenance.SpecialLeagueSetting.Constants;

namespace Maintenance.SpecialLeagueSetting.Request;

public class UpdateSettingRequest
{
	public int LeagueId { get; set; }
	public byte AutoType { get; set; }

	public byte SportType { get; set; }
	public string Action { get; set; } = "Update";
}


public class UpdateSettingRequestValidator : AbstractValidator<UpdateSettingRequest>
{
	public UpdateSettingRequestValidator()
	{
		RuleFor(x => x.LeagueId).NotEmpty().WithMessage("LeagueId is required");
		RuleFor(x => x.AutoType).IsInEnum().WithMessage("AutoType is required");
		RuleFor(x => x.SportType).IsInEnum().WithMessage("SportType is required");
		RuleFor(x => x.Action).IsInEnum().WithMessage("Action is required");
	}
}

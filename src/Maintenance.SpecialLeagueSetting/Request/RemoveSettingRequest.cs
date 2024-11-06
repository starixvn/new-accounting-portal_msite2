using FluentValidation;
using Maintenance.SpecialLeagueSetting.Constants;

namespace Maintenance.SpecialLeagueSetting.Request;

public class RemoveSettingRequest
{
    public int LeagueId { get; set; }
    public byte AutoType { get; set; }
}


public class RemoveSettingRequestValidator : AbstractValidator<RemoveSettingRequest>
{
    public RemoveSettingRequestValidator()
    {
        RuleFor(x => x.LeagueId).NotEmpty().WithMessage("LeagueId is required");
        RuleFor(x => x.AutoType)
            .Must(x => x is AutoTypeSetting.DirectSabaSoccer or AutoTypeSetting.FantasySoccer or AutoTypeSetting.FantasyBasketball);
    }
}

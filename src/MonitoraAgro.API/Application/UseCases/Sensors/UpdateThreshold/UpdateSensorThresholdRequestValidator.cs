using FluentValidation;
using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors;

namespace MonitoraAgro.API.Application.UseCases.Sensors.UpdateThreshold;

public class UpdateSensorThresholdRequestValidator : AbstractValidator<UpdateSensorThresholdRequest>
{
    public UpdateSensorThresholdRequestValidator()
    {
        RuleFor(x => x.NotificationEnabled)
            .NotNull().WithMessage(ErrorMessages.NOTIFICATION_ENABLED_REQUIRED);

        RuleFor(x => x)
            .Must(x => !(x.LowerLimit.HasValue && x.UpperLimit.HasValue && x.LowerLimit.Value > x.UpperLimit.Value))
            .WithMessage(ErrorMessages.LOWER_LIMIT_GREATER_THAN_UPPER_LIMIT);
    }
}

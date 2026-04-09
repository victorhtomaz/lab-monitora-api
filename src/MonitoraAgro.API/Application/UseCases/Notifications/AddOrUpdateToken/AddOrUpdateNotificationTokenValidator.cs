using FluentValidation;
using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors;

namespace MonitoraAgro.API.Application.UseCases.Notifications.AddOrUpdateToken;

public class AddOrUpdateNotificationTokenValidator : AbstractValidator<RegisterNotificationTokenRequest>
{
    public AddOrUpdateNotificationTokenValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage(ErrorMessages.NOTIFICATION_TOKEN_REQUIRED);
    }
}
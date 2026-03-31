using FluentValidation;
using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Domain.Extensions;

namespace MonitoraAgro.API.Application.UseCases.Authentication.DoLogin;

public class DoLoginValidator : AbstractValidator<LoginRequest>
{
    public DoLoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage(ErrorMessages.EMAIL_REQUIRED);
        When(x => x.Email.NotEmpty(), () =>
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(ErrorMessages.INVALID_EMAIL);
        });
    }
}

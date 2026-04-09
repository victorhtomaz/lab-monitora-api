using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors.Exceptions;

namespace MonitoraAgro.API.Application.UseCases.Notifications.AddOrUpdateToken;

public class AddOrUpdateNotificationTokenUseCase : IAddOrUpdateNotificationTokenUseCase
{
    public Task Execute(RegisterNotificationTokenRequest request)
    {
        Validate(request);


        return Task.CompletedTask;
    }

    private static void Validate(RegisterNotificationTokenRequest request)
    {
        var validator = new AddOrUpdateNotificationTokenValidator();
        var result = validator.Validate(request);
        
        if (result.IsValid is false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Services.LoggedUser;

namespace MonitoraAgro.API.Application.UseCases.Notifications.AddOrUpdateToken;

public class AddOrUpdateNotificationTokenUseCase : IAddOrUpdateNotificationTokenUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly INotificationTokenRepository _notificationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AddOrUpdateNotificationTokenUseCase(ILoggedUser loggedUser, 
        INotificationTokenRepository notificationTokenRepository, 
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _notificationTokenRepository = notificationTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RegisterNotificationTokenRequest request)
    {
        Validate(request);

        var user = await _loggedUser.GetUser();

        var notificationToken = await _notificationTokenRepository.GetByInstallationId(request.InstallationId);
        
        if (notificationToken == null)
        {

            notificationToken = new NotificationToken
            {
                UserId = user.Id,
                InstallationId = request.InstallationId,
                Token = request.Token,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _notificationTokenRepository.Add(notificationToken);
        }
        else
        {
            notificationToken.UserId = user.Id;
            notificationToken.Token = request.Token;
            notificationToken.UpdatedAt = DateTime.UtcNow;

            _notificationTokenRepository.Update(notificationToken);
        }

        await _unitOfWork.Commit();
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
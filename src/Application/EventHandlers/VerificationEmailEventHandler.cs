/*
* Author: Steve Bang
* History:
* - [2025-04-19] - Created by mrsteve.bang@gmail.com
*/

using Steve.ManagerHero.UserService;
using Steve.ManagerHero.UserService.Helpers;

public class RegistrationEventHandler(
    IEmailService _emailService,
    ProjectSettings _projectSettings,
    IConfiguration _configuration
) : INotificationHandler<RegistrationEvent>
{
    public Task Handle(RegistrationEvent notification, CancellationToken cancellationToken)
    {
        var user = notification.User;

        string encryptToken = EncryptionAESHelper.EncryptObject<object>(
            new
            {
                UserId = user.Id
            },
            _configuration.GetValue<string>("EncryptionSecretKey") ?? "Default_Key",
            EncryptionPurpose.VerificationEmailAddress
        );

        // Build link
        UrlBuilder uriBuilder = new UrlBuilder(_projectSettings.VerificationLink)
        .AddQueryParameter("token", encryptToken);

        _emailService.SendRegistrationEmailAsync(
            user.EmailAddress.Value,
            uriBuilder.ToString()
        );

        return Task.CompletedTask;
    }
}
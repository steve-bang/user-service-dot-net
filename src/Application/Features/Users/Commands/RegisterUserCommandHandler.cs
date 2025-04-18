/*
* Author: Steve Bang
* History:
* - [2025-04-16] - Created by mrsteve.bang@gmail.com
*/

namespace Steve.ManagerHero.Application.Features.Users.Commands;

public class RegisterUserCommandHandler(
    IUserRepository _userRepository
) : IRequestHandler<RegisterUserCommand, UserDto>
{
    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        bool isExistsEmail = await _userRepository.IsExistEmailAsync(request.EmailAddress, cancellationToken);

        if (isExistsEmail) throw ExceptionProviders.User.EmailAlreadyExistsException;


        User user = User.Create(
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.EmailAddress,
            password: request.Password
        );

        User userCreated = await _userRepository.CreateAsync(user, cancellationToken);

        await _userRepository.UnitOfWork.SaveEntitiesAsync();

        return new UserDto(
            Id: userCreated.Id,
            EmailAddress: userCreated.EmailAddress.Value,
            FirstName: userCreated.FirstName,
            LastName: userCreated.LastName,
            DisplayName: userCreated.DisplayName,
            SecondaryEmailAddress: userCreated.SecondaryEmailAddress != null ? userCreated.SecondaryEmailAddress.Value : null,
            PhoneNumber: userCreated.PhoneNumber != null ? userCreated.PhoneNumber.Value : null,
            LastLogin: userCreated.LastLoginDate,
            Address: userCreated.Address,
            IsActive: userCreated.IsActive,
            IsEmailVerified: userCreated.IsEmailVerified,
            IsPhoneVerified: userCreated.IsPhoneVerified
        );
    }
}

/*
* Author: Steve Bang
* History:
* - [2025-04-16] - Created by mrsteve.bang@gmail.com
*/

using Steve.ManagerHero.UserService.Domain.ValueObjects;

namespace Steve.ManagerHero.UserService.Application.DTO;

public record UserDto(
    Guid Id,
    string EmailAddress,
    string FirstName,
    string LastName,
    string DisplayName,
    string? SecondaryEmailAddress,
    string? PhoneNumber,
    DateTime? LastLogin,
    Address? Address,
    bool IsActive,
    bool IsEmailVerified,
    bool IsPhoneVerified
);
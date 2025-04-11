/*
* Author: Steve Bang
* History:
* - [2024-04-11] - Created by mrsteve.bang@gmail.com
*/

namespace Steve.ManagerHero.UserService.Application.DTO;

public record UpdateRolePermissionsDto(
    Guid RoleId,
    IEnumerable<Guid> PermissionIds);
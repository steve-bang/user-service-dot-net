/*
* Author: Steve Bang
* History:
* - [2025-04-11] - Created by mrsteve.bang@gmail.com
*/

using Steve.ManagerHero.UserService.Domain.Common;

namespace Steve.ManagerHero.UserService.Infrastructure.Repository;

public class PermissionRepository(
    UserAppContext _context
) : IPermissionRepository
{
    public IUnitOfWork UnitOfWork => _context;
}
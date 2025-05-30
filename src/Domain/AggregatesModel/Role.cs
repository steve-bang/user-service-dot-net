/*
* Author: Steve Bang
* History:
* - [2025-04-11] - Created by mrsteve.bang@gmail.com
*/

namespace Steve.ManagerHero.UserService.Domain.AggregatesModel;

public class Role : AggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    private readonly List<RolePermission> _rolePermissions = new();
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public Role() {}

    public Role(string name, string description)
    {
        Name = name;
        Description = description ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description ?? string.Empty;
    }

    public void AddPermissions(IEnumerable<Permission> permissions)
    {
        foreach (var permission in permissions)
        {
            if (!_rolePermissions.Any(rp => rp.PermissionId == permission.Id))
            {
                _rolePermissions.Add(new RolePermission(this, permission));
            }
        }
    }

    public void RemovePermissions(IEnumerable<Permission> permissions)
    {
        foreach (var permission in permissions)
        {
            var rolePermission = _rolePermissions.FirstOrDefault(rp => rp.PermissionId == permission.Id);
            if (rolePermission != null)
            {
                _rolePermissions.Remove(rolePermission);
            }
        }
    }

    public bool HasPermission(string permissionName)
    {
        return _rolePermissions.Any(rp =>
            string.Equals(rp.Permission.Name, permissionName, StringComparison.OrdinalIgnoreCase));
    }
}
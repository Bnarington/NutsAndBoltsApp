using Infrastructure.Repositories;
using Infrastructure.Entities;

public class RoleService
{
    private readonly RoleRepository _roleRepository;

    public RoleService(RoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public string GetOrCreateRole(string roleName)
    {
        var roleEntity = _roleRepository.SelectRoleName(roleName);

        if (roleEntity == null)
        {
            roleEntity = new RoleEntity()
            {
                RoleName = roleName,
            };
            _roleRepository.Create(roleEntity); // Save the new role to the repository
            return roleName;
        }

        return roleEntity.RoleName; // Return the role name of the existing role entity
    }
}

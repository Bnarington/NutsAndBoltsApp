using Infrastructure.Contexts;
using Infrastructure.Repositories;
using System.ComponentModel;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, RoleRepository roleRepository)
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;

    public bool CreateUser(string firstName, string lastName, string email, string password, string phonenumber, string roleName)
    {
        if ( )
        {

        }
    }
}

public class RoleService(RoleRepository roleRepository, UserRepository userRepository)
{
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    public int GetOrCreateRole(string roleName)
    {
        var roleEntity = _roleRepository.Select(roleName);
        if (roleEntity == null)
        {
            return _roleRepository.Insert(roleName);
        }

        return roleEntity.Id;
    }


    public int GetRoleId()
    {

        try
        {
            
        }


        var roleName = _userRepository.SelectRoleName().Count() == 0 ? "Administrator" : "User";
        return GetOrCreateRole(roleName);
    }
}
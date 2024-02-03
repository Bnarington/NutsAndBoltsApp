using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.ComponentModel;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, RoleRepository roleRepository)
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;

    public bool CreateUser(string firstName, string lastName, string email, string password, string phonenumber, string roleName)
    {
        if (!_userRepository.Exists(x => x.Email == email))
        {

            var role = _roleRepository.SelectRoleName(roleName);

            var userEntity = new UserEntity()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                PhoneNumber = phonenumber,
                Role = (RoleEntity)role
            };

            var result = _userRepository.Create(userEntity);
            if (result != null)
            {
                return true;
            }

        }
        return false;
    }
}

public class RoleService(RoleRepository roleRepository, UserRepository userRepository)
{
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    public string GetOrCreateRole(string roleName)
    {
        var roleEntity = _roleRepository.SelectRoleName(roleName);
        if (roleEntity == null)
        {
           new RoleEntity()
            {
                RoleName = roleName,
            };

            return roleName;
        }

        return null!;
    }
}
using Infrastructure.Contexts;
using Infrastructure.DTOs;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.ComponentModel;
using System.Diagnostics;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, RoleRepository roleRepository)
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;

    //public bool CreateUser(string firstName, string lastName, string email, string password, string phonenumber, string roleName)
    //{
    //    if (!_userRepository.Exists(x => x.Email == email))
    //    {

    //        var role = _roleRepository.SelectRoleName(roleName);

    //        var userEntity = new UserEntity()
    //        {
    //            FirstName = firstName,
    //            LastName = lastName,
    //            Email = email,
    //            Password = password,
    //            PhoneNumber = phonenumber,
    //            Role = (RoleEntity)role
    //        };

    //        var result = _userRepository.Create(userEntity);
    //        if (result != null)
    //        {
    //            return true;
    //        }

    //    }
    //    return false;
    //}

    public UserEntity CreateUser(string firstName, string lastName, string email, string password, string phoneNumber, string roleName)
    {
        // Validate input parameters
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(roleName))
        {
            throw new ArgumentException("One or more input parameters are null or empty.");
        }

        // Check if a user with the provided email already exists
        if (!_userRepository.Exists(x => x.Email == email))
        {
            // Retrieve role entity based on role name
            var role = _roleRepository.SelectRoleName(roleName);
            if (role == null)
            {
                throw new ArgumentException("Invalid role name.");
            }

            // Create user entity
            var userEntity = new UserEntity()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                PhoneNumber = phoneNumber,
                Role = (RoleEntity)role
            };

            // Save user entity to the database
            var result = _userRepository.Create(userEntity);
            if (result != null)
            {
                return result; // Return the newly created user entity
            }
            else
            {
                throw new Exception("Failed to create user.");
            }
        }
        else
        {
            throw new InvalidOperationException("A user with the provided email already exists.");
        }
    }

    public IEnumerable<User> GetAllUsers()
    {

        var users = new List<User>();

        try
        {
            var result = _userRepository.GetAll();


            foreach (var user in result)
            {
                users.Add(new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    RoleName = user.Role.RoleName
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return users;
    }

    public IEnumerable<User> GetOneUser(User userEntity)
    {

        var users = new List<User>();

        try
        {
            var result = _userRepository.GetOne(x => x.Email == userEntity.Email);
            if (result != null)
            {
                var user = new User();
                users.Add(new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    RoleName = user.RoleName
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return users;
    }

    public bool UpdateUser(User userEntity)
    {
        try
        {
            var product = _userRepository.GetOne(x => x.Email == userEntity.Email);
            if (product != null)
            {
                var productToUpdate = _userRepository.Update(product);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }

    public bool DeleteUser(User userEntity)
    {

        try
        {
            var product = _userRepository.GetOne(x => x.Email == userEntity.Email);
            if (product != null)
            {
                var productToDelte = _userRepository.Delete(x => x.Email == product.Email);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }
}

//public class RoleService(RoleRepository roleRepository)
//{
//    private readonly RoleRepository _roleRepository = roleRepository;
//    public string GetOrCreateRole(string roleName)
//    {
//        var roleEntities = _roleRepository.SelectRoleName(roleName);

//        if (roleEntity == null)
//        {
//            roleEntity = new RoleEntity()
//            {
//                RoleName = roleName,
//            };
//            _roleRepository.Create(roleEntity); // Save the new role to the repository
//            return roleName;
//        }

//        return roleEntity.RoleName; // Return the role name of the existing role entity
//    }
//}

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

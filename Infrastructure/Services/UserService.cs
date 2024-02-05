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

    public IEnumerable<User> GetOneProduct(User userEntity)
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

    public bool UpdateProduct(Product productEntity)
    {
        try
        {
            var product = _userRepository.GetOne(x => x.Email == productEntity.Email);
            if (product != null)
            {
                var productToUpdate = _userRepository.Update(product);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }

    public bool DeleteProduct(User userEntity)
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

public class RoleService(RoleRepository roleRepository)
{
    private readonly RoleRepository _roleRepository = roleRepository;
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
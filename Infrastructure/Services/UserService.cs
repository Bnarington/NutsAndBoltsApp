using Infrastructure.DTOs;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Infrastructure.Services
{
    /// <summary>
    /// Service class responsible for managing user-related operations.
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class with the provided repositories.
        /// </summary>
        /// <param name="userRepository">The repository for user entities.</param>
        /// <param name="roleRepository">The repository for role entities.</param>
        public UserService(UserRepository userRepository, RoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Creates a new user entity with the specified details.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        /// <param name="roleName">The role name of the user.</param>
        /// <returns>The newly created user entity.</returns>
        /// <exception cref="ArgumentException">Thrown when one or more input parameters are null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a user with the provided email already exists.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided role name is invalid.</exception>
        /// <exception cref="Exception">Thrown when failed to create a user.</exception>
        public UserEntity CreateUser(string firstName, string lastName, string email, string password, string phoneNumber, string roleName)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("One or more input parameters are null or empty.");
            }

            // Check if a user with the provided email already exists
            if (_userRepository.Exists(x => x.Email == email))
            {
                throw new InvalidOperationException("A user with the provided email already exists.");
            }

            // Retrieve role entity based on role name
            var roleEntity = _roleRepository.SelectRoleName(roleName);
            if (roleEntity == null)
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
                Role = roleEntity
            };

            // Save user entity to the database
            var result = _userRepository.Create(userEntity);
            if (result == null)
            {
                throw new Exception("Failed to create user.");
            }

            return result; // Return the newly created user entity
        }

        /// <summary>
        /// Retrieves all users and converts them to DTOs.
        /// </summary>
        /// <returns>An enumerable collection of user DTOs.</returns>
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
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
            }

            return users;
        }

        /// <summary>
        /// Retrieves a single user based on the provided user entity and converts it to a DTO.
        /// </summary>
        /// <param name="userEntity">The user entity to retrieve.</param>
        /// <returns>The user DTO if found; otherwise, null.</returns>
        public User GetOneUser(User userEntity)
        {
            var result = _userRepository.GetOne(x => x.Email == userEntity.Email);
            if (result != null)
            {
                return new User
                {
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Email = result.Email,
                    Password = result.Password,
                    PhoneNumber = result.PhoneNumber,
                    RoleName = result.Role.RoleName
                };
            }

            return null;
        }

        /// <summary>
        /// Updates an existing user entity with the provided details.
        /// </summary>
        /// <param name="userEntity">The updated user entity.</param>
        /// <returns>True if the user was successfully updated; otherwise, false.</returns>
        public bool UpdateUser(User userEntity)
        {
            try
            {
                var user = _userRepository.GetOne(x => x.Email == userEntity.Email);
                if (user != null)
                {
                    user.FirstName = userEntity.FirstName;
                    user.LastName = userEntity.LastName;
                    user.Password = userEntity.Password;
                    user.PhoneNumber = userEntity.PhoneNumber;
                    user.Role = _roleRepository.SelectRoleName(userEntity.RoleName);

                    _userRepository.Update(x => x.Email == userEntity.Email, user);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Deletes an existing user entity.
        /// </summary>
        /// <param name="userEntity">The user entity to delete.</param>
        /// <returns>True if the user was successfully deleted; otherwise, false.</returns>
        public bool DeleteUser(UserEntity userEntity)
        {
            try
            {
                var user = _userRepository.GetOne(x => x.Email == userEntity.Email);
                if (user != null)
                {
                    _userRepository.Delete(x => x.Email == userEntity.Email); // Use a predicate to find the user to delete
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
            }

            return false;
        }
    }
}

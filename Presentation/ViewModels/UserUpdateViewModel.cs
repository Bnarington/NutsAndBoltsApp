using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.DTOs;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Presentation.ViewModels
{
    /// <summary>
    /// ViewModel responsible for updating user information.
    /// </summary>
    public partial class UserUpdateViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly IServiceProvider _serviceProvider;
        private readonly RoleService _roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserUpdateViewModel"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        /// <param name="roleService">The role service.</param>
        public UserUpdateViewModel(IServiceProvider serviceProvider, RoleService roleService)
        {
            _serviceProvider = serviceProvider;
            _userService = serviceProvider.GetRequiredService<UserService>()!;
            _roleService = roleService;

            LoadRoles();
        }

        private UserModel _selectedUser;
        /// <summary>
        /// Gets or sets the selected user.
        /// </summary>
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        [RelayCommand]
        private void NavigateToUserList()
        {
            // Navigate back to the user list view
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
        }

        [RelayCommand]
        public void UpdateUser()
        {
            try
            {
                // Check if a user is selected
                if (SelectedUser != null)
                {
                    // Convert UserModel to User entity
                    var user = new User
                    {
                        FirstName = SelectedUser.FirstName,
                        LastName = SelectedUser.LastName,
                        Email = SelectedUser.Email,
                        Password = SelectedUser.Password,
                        PhoneNumber = SelectedUser.PhoneNumber,
                        RoleName = SelectedUser.RoleName,
                    };

                    // Update the user
                    var result = _userService.UpdateUser(user);

                    if (result)
                    {
                        // Show success message and reload the user list
                        MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        var userListViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
                        userListViewModel.ShowUserList();
                        NavigateToUserList();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update user. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No user selected for update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Debug.WriteLine("ERROR :: " + ex.Message);
                MessageBox.Show("An error occurred while updating user. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<RoleModel> Roles { get; } = new ObservableCollection<RoleModel>();

        private RoleModel _selectedRole;
        /// <summary>
        /// Gets or sets the selected role.
        /// </summary>
        public RoleModel SelectedRole
        {
            get { return _selectedRole; }
            set { SetProperty(ref _selectedRole, value); }
        }

        /// <summary>
        /// Loads available roles.
        /// </summary>
        private void LoadRoles()
        {
            try
            {
                var roleNames = new string[] { "Admin", "User", "Super User" }; // Example role names to retrieve or create
                foreach (var roleName in roleNames)
                {
                    var role = _roleService.GetOrCreateRole(roleName);
                    if (role != null)
                    {
                        Roles.Add(new RoleModel { RoleName = roleName }); // Set only the RoleName property
                    }
                }
            }
            catch (Exception ex)
            {
                // Show error message if roles cannot be loaded
                MessageBox.Show("An error occurred while loading roles: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

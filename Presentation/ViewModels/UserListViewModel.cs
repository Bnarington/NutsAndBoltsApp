using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Presentation.Models.Extensions;
using Infrastructure.Repositories;

namespace Presentation.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing user list view.
    /// </summary>
    public partial class UserListViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly UserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserListViewModel"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        /// <param name="userService">The user service.</param>
        public UserListViewModel(IServiceProvider serviceProvider, UserService userService, UserRepository userRepository)
        {
            _serviceProvider = serviceProvider;
            _userService = userService;
            _userRepository = userRepository;
            ShowUserList();
        }

        [RelayCommand]
        private void NavigateToAddUser()
        {
            // Navigate to the add user view
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserAddViewModel>();
        }

        [RelayCommand]
        private void NavigateToMainMenu()
        {
            var mainViewModel = _serviceProvider.GetService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<MainMenuViewModel>();
        }

        [RelayCommand]
        private void NavigateToUpdateUser(UserModel selectedUser)
        {
            // Navigate to the update user view if a user is selected
            if (selectedUser != null)
            {
                var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                var userUpdateViewModel = _serviceProvider.GetRequiredService<UserUpdateViewModel>();
                userUpdateViewModel.SelectedUser = selectedUser; // Pass the selected user to the update view model
                mainViewModel.CurrentViewModel = userUpdateViewModel;
            }
            else
            {
                MessageBox.Show("Please select a user to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<UserModel> UserList { get; } = new ObservableCollection<UserModel>();

        /// <summary>
        /// Displays the list of users.
        /// </summary>
        public void ShowUserList()
        {
            try
            {
                UserList.Clear(); // Clear the existing list before adding new users

                var users = _userService.GetAllUsers();

                foreach (var user in users)
                {
                    UserList.Add(new UserModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password,
                        PhoneNumber = user.PhoneNumber,
                        RoleName = user.RoleName,
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading users: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private UserModel _selectedUser;
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }


        [RelayCommand]
        private void DeleteUser(UserModel user)
        {
            try
            {
                if (user != null)
                {
                    // Delete the user entity based on email
                    _userRepository.Delete(u => u.Email == user.Email);

                    // Remove the user from the user list if deletion is successful
                    UserList.Remove(user);
                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No user selected for deletion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
                MessageBox.Show("An error occurred while deleting the user. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

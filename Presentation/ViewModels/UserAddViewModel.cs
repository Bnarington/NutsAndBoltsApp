using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.DTOs;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation.ViewModels;

public partial class UserAddViewModel : ObservableObject
{
    private readonly RoleService _roleService;
    private readonly IServiceProvider _sp;

    public UserAddViewModel(IServiceProvider sp, RoleService rs)
    {
        _sp = sp;
        _roleService = rs;
        LoadRoles();
    }

    public ObservableCollection<RoleModel> Roles { get; } = new ObservableCollection<RoleModel>();

    private RoleModel _selectedRole;
    public RoleModel SelectedRole
    {
        get { return _selectedRole; }
        set { SetProperty(ref _selectedRole, value); }
    }

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
            MessageBox.Show("An error occurred while loading roles: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void NavigateToUserList()
    {
        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
    }

    [ObservableProperty]
    private UserModel _userForm = new();
    [ObservableProperty]
    private ObservableCollection<UserModel> _userList = [];

    [RelayCommand]
    public void CreateUser()
    {
        try
        {
            var userService = _sp.GetRequiredService<UserService>();

            var newUser = userService.CreateUser(UserForm.FirstName, UserForm.LastName, UserForm.Email, UserForm.Password, UserForm.PhoneNumber!, SelectedRole.RoleName);
            if (newUser != null)
            {
                NavigateToUserList();
            }
            else
            {
                MessageBox.Show("User creation failed. Please check your input and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex) { MessageBox.Show("An error occurred while creating a user. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

    }


}





using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Models;
using Presentation.Views;
using System.Collections.ObjectModel;

namespace Presentation.ViewModels;

public partial class UserListViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;

    public UserListViewModel(IServiceProvider sp)
    {
        _sp = sp;
        ShowUserList();
    }

    [RelayCommand]
    private void NavigateToAddUser()
    {
        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserAddViewModel>();
    }

    [ObservableProperty]
    private UserModel _userForm = new();
    [ObservableProperty]
    private ObservableCollection<UserModel> _userList = [];


    public void ShowUserList()
    {
        var userService = _sp.GetRequiredService<UserService>();

        var users = userService.GetAllUsers();

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
}


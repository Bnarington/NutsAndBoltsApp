using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Models;
using System.Collections.ObjectModel;

namespace Presentation.ViewModels;

public partial class UserListViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;

    public UserListViewModel(IServiceProvider sp)
    {
        _sp = sp;
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

    [RelayCommand]
    public void AddUserToDB()
    {
        if (!string.IsNullOrWhiteSpace(UserForm.Email) && !string.IsNullOrWhiteSpace(UserForm.Password))
        {
            UserList.Add(UserForm);
            UserForm = new();
        }
    }

    public string UserFullName (UserModel model)
    {
        var firstName = model.FirstName;
        var lastName = model.LastName;

        return firstName + " " + lastName;
    }
}

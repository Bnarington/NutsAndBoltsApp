using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Models;
using System.Collections.ObjectModel;

namespace Presentation.ViewModels;

public partial class UserAddViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;

    public UserAddViewModel(IServiceProvider sp)
    {
        _sp = sp;
    }

    [RelayCommand]
    private void NavigateToUserList()
    {
        var mainViewModel = _sp.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _sp.GetRequiredService<UserListViewModel>();
    }
}

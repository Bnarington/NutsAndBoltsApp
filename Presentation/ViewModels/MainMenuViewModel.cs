using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Presentation.ViewModels
{
    public partial class MainMenuViewModel : ObservableObject
    {
        public readonly IServiceProvider _ServiceProvider;

        public MainMenuViewModel(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
        }

        [RelayCommand]
        public void NavigateToProductList()
        {
            var mainViewModel = _ServiceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _ServiceProvider.GetRequiredService<ProductListViewModel>();
        }

        [RelayCommand]
        public void NavigateToUserList()
        {
            var mainViewModel = _ServiceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _ServiceProvider.GetRequiredService<UserListViewModel>();
        }

        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}

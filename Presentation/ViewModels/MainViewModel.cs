using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableObject? _currentViewModel;

        private readonly IServiceProvider _sp;

        public MainViewModel(IServiceProvider sp)
        {
            _sp = sp;
            CurrentViewModel = _sp.GetRequiredService<MainMenuViewModel>();
        }

    }
}

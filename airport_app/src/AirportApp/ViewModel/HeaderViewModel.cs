using System.ComponentModel;
using System.Runtime.CompilerServices;

using AirportApp.WinUI.Utils;

using CommunityToolkit.Mvvm.Input;

namespace AirportApp.ViewModel
{
    public partial class HeaderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly INavigationUtil navigationUtil;

        public HeaderViewModel(INavigationUtil navigationUtil)
        {
            this.navigationUtil = navigationUtil;
        }

        [RelayCommand]
        private void NavigateHome()
        {
            navigationUtil.NavigateToHome();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

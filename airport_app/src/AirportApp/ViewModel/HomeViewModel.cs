using System.ComponentModel;
using System.Runtime.CompilerServices;

using AirportApp.WinUI.Utils;

using CommunityToolkit.Mvvm.Input;

namespace AirportApp.ViewModel
{
    public partial class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly INavigationUtil navigationUtil;

        public HomeViewModel(INavigationUtil navigationUtil)
        {
            this.navigationUtil = navigationUtil;
        }

        [RelayCommand]
        private void NavigateToCompany()
        {
            this.navigationUtil.NavigateToSelectCompany();
        }

        [RelayCommand]
        private void NavigateToAdmin()
        {
            this.navigationUtil.NavigateToAirportAdmin();
        }

        [RelayCommand]
        private void NavigateToStaff()
        {
            this.navigationUtil.NavigateToStaffLogin();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

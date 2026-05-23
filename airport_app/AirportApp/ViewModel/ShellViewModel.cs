using System.ComponentModel;
using System.Runtime.CompilerServices;

using AirportApp.WinUI.Utils;

using CommunityToolkit.Mvvm.Input;

namespace AirportApp.ViewModel
{
    public partial class ShellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly INavigationUtil navigationUtil;
        private bool isAirportTabActive = true;

        public bool IsAirportTabActive
        {
            get => isAirportTabActive;
            set => SetProperty(ref isAirportTabActive, value);
        }

        public ShellViewModel(INavigationUtil navigationUtil)
        {
            this.navigationUtil = navigationUtil;
        }

        [RelayCommand]
        private void NavigateToAirport()
        {
            navigationUtil.NavigateToConfiguredAirportRole();
            IsAirportTabActive = true;
        }

        [RelayCommand]
        private void NavigateToShop()
        {
            navigationUtil.NavigateToConfiguredDutyFreeRole();
            IsAirportTabActive = false;
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

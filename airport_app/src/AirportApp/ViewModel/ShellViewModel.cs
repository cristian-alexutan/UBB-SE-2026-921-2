using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using AirportApp.WinUI.Services;

namespace AirportApp.ViewModel
{
    public partial class ShellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly INavigationService navigationService;
        private bool isAirportTabActive = true;

        public bool IsAirportTabActive
        {
            get => isAirportTabActive;
            set => SetProperty(ref isAirportTabActive, value);
        }

        public ShellViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        [RelayCommand]
        private void NavigateToAirport()
        {
            navigationService.NavigateToHome();
            IsAirportTabActive = true;
        }

        [RelayCommand]
        private void NavigateToShop()
        {
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

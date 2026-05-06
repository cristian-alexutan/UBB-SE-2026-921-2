using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using AirportApp.WinUI.Services;
using AirportApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace AirportApp.WinUI
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; private set; }

        public ShellPage()
        {
            this.InitializeComponent();

            var navigationService = App.Services.GetRequiredService<INavigationService>();
            ViewModel = new ShellViewModel(navigationService);
            this.DataContext = ViewModel;

            navigationService.Initialize(ContentFrame);

            navigationService.NavigateToHome();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShellViewModel.IsAirportTabActive))
            {
                if (!ViewModel.IsAirportTabActive)
                {
                    ContentFrame.Navigate(typeof(LandingPage));
                }
            }
        }
    }
}

using AirportApp.WinUI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AirportApp.WinUI.ModuleSelection
{
    public sealed partial class ModuleSelectionPage : Page
    {
        public ModuleSelectionPage()
        {
            this.InitializeComponent();
        }

        private void AirportMgmtButton_Click(object sender, RoutedEventArgs e)
        {
            var nav = App.Services.GetRequiredService<INavigationService>();
            nav.NavigateToHome();
        }

        private void DutyFreeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LandingPage));
        }
    }
}

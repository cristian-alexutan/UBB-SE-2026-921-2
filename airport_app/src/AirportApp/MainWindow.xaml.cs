using Microsoft.UI.Xaml;
using AirportApp.WinUI.Services;

namespace AirportApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow(INavigationService navigationService)
        {
            this.InitializeComponent();

            navigationService.Initialize(RootFrame);
            navigationService.NavigateToModuleSelection();
        }
    }
}
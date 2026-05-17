using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using AirportApp.WinUI.Utils;
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

            var navigationUtil = App.Services.GetRequiredService<INavigationUtil>();
            ViewModel = new ShellViewModel(navigationUtil);
            this.DataContext = ViewModel;

            navigationUtil.Initialize(ContentFrame);

            navigationUtil.NavigateToConfiguredAirportRole();
        }
    }
}

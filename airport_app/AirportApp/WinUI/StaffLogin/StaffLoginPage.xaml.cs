using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using AirportApp.ViewModel;

namespace AirportApp.WinUI.StaffLogin
{
    public sealed partial class StaffLoginPage : Page
    {
        public StaffLoginViewModel ViewModel { get; private set; } = null!;

        public StaffLoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is StaffLoginViewModel vm)
            {
                ViewModel = vm;
                DataContext = ViewModel;
            }
        }
    }
}
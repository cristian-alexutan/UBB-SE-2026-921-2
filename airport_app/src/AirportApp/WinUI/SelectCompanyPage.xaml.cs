using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using AirportApp.ViewModel;

namespace AirportApp.WinUI
{
    public sealed partial class SelectCompanyPage : Page
    {
        public SelectCompanyViewModel ViewModel { get; private set; } = null!;

        public SelectCompanyPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is SelectCompanyViewModel vm)
            {
                ViewModel = vm;
                DataContext = ViewModel;
                Bindings.Update();
            }
        }
    }
}
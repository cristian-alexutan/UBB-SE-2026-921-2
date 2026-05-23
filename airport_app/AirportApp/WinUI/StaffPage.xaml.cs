using AirportApp.ViewModel;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace AirportApp.WinUI
{
    public sealed partial class StaffPage : Page
    {
        public StaffPageViewModel ViewModel { get; private set; } = null!;

        public StaffPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ValueTuple<StaffPageViewModel, int> context)
            {
                ViewModel = context.Item1;
                DataContext = ViewModel;
                ViewModel.Initialize(context.Item2);
            }
        }
    }
}
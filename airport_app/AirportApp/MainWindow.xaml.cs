using Microsoft.UI.Xaml;
using AirportApp.WinUI;

namespace AirportApp
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            RootFrame.Navigate(typeof(ShellPage));
        }
    }
}
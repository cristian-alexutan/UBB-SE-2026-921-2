using Microsoft.UI.Xaml.Controls;

namespace AirportApp.WinUI.Services
{
    public interface INavigationService
    {
        void Initialize(Frame frame);
        void NavigateToModuleSelection();
        void NavigateToHome();
        void NavigateToSelectCompany();
        void NavigateToCompanyDashboard(int companyId);
        void NavigateToStaffLogin();
        void NavigateToAirportAdmin();
        void NavigateToStaffDashboard(int employeeId);
    }
}

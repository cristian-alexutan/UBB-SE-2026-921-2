using Microsoft.UI.Xaml.Controls;

namespace AirportApp.WinUI.Utils
{
    public interface INavigationUtil
    {
        void Initialize(Frame frame);
        void NavigateToHome();
        void NavigateToConfiguredAirportRole();
        void NavigateToConfiguredDutyFreeRole();
        void NavigateToSelectCompany();
        void NavigateToCompanyDashboard(int companyId);
        void NavigateToStaffLogin();
        void NavigateToAirportAdmin();
        void NavigateToStaffDashboard(int employeeId);
    }
}

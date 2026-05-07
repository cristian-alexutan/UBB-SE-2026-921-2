using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

using AirportApp.Data.Services;
using AirportApp.Data.User;
using AirportApp.ViewModel;
using AirportApp.WinUI.AirportAdmin;
using AirportApp.WinUI.StaffLogin;

namespace AirportApp.WinUI.Utils
{
    public class NavigationUtil : INavigationUtil
    {
        private readonly IServiceProvider services;
        private readonly MockUserUtil mockUserUtil;
        private readonly UserSession userSession;
        private Frame frame;

        public NavigationUtil(IServiceProvider services, MockUserUtil mockUserUtil, UserSession userSession)
        {
            this.services = services;
            this.mockUserUtil = mockUserUtil;
            this.userSession = userSession;
        }

        public void Initialize(Frame frame)
        {
            this.frame = frame;
        }

        public void NavigateToHome()
        {
            frame.Navigate(typeof(HomePage), services.GetRequiredService<HomeViewModel>());
        }

        public void NavigateToConfiguredAirportRole()
        {
            MockUserRoles roles = mockUserUtil.GetRolesForUser(App.ConfiguredUserId);

            switch (roles.AirportRole)
            {
                case AirportModuleRole.None:
                    frame.Navigate(typeof(NoAirportRolePage));
                    break;
                case AirportModuleRole.CompanyRepresentative:
                    NavigateToCompanyDashboard(roles.CompanyId ?? 1);
                    break;
                case AirportModuleRole.AirportAdministrator:
                    NavigateToAirportAdmin();
                    break;
                case AirportModuleRole.AirportStaffMember:
                    NavigateToStaffDashboard(roles.EmployeeId ?? App.ConfiguredUserId);
                    break;
                default:
                    NavigateToHome();
                    break;
            }
        }

        public void NavigateToConfiguredDutyFreeRole()
        {
            MockUserRoles roles = mockUserUtil.GetRolesForUser(App.ConfiguredUserId);

            if (roles.DutyFreeRole == DutyFreeModuleRole.Manager)
            {
                userSession.SetAdmin(roles.DutyFreeUserId);
            }
            else
            {
                userSession.SetClient(roles.DutyFreeUserId);
            }

            frame.Navigate(typeof(ShopPage));
        }

        public void NavigateToSelectCompany()
        {
            frame.Navigate(typeof(SelectCompanyPage), services.GetRequiredService<SelectCompanyViewModel>());
        }

        public void NavigateToCompanyDashboard(int companyId)
        {
            frame.Navigate(typeof(CompanyPage), (services.GetRequiredService<CompanyViewModel>(), companyId));
        }

        public void NavigateToStaffLogin()
        {
            MockUserRoles roles = mockUserUtil.GetRolesForUser(App.ConfiguredUserId);
            NavigateToStaffDashboard(roles.EmployeeId ?? App.ConfiguredUserId);
        }

        public void NavigateToAirportAdmin()
        {
            frame.Navigate(
                typeof(AirportAdminPage),
                (
                    services.GetRequiredService<AirportAdminViewModel>(),
                    services.GetRequiredService<FlightsDashboardViewModel>(),
                    services.GetRequiredService<EmployeesDashboardViewModel>(),
                    services.GetRequiredService<AirportDashboardViewModel>()));
        }

        public void NavigateToStaffDashboard(int employeeId)
        {
            frame.Navigate(typeof(StaffPage), (services.GetRequiredService<StaffPageViewModel>(), employeeId));
        }
    }
}

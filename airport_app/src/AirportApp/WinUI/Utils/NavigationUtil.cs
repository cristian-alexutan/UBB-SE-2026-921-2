using AirportApp.ViewModel;
using AirportApp.WinUI.AirportAdmin;

using AirportLib.Domain.User;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace AirportApp.WinUI.Utils
{
    public class NavigationUtility(
        IServiceProvider serviceProvider,
        MockUserUtility mockUserUtility,
        UserSession userSession) : INavigationUtil
    {
        private Frame? rootNavigationFrame;

        public void Initialize(Frame navigationFrame)
        {
            this.rootNavigationFrame = navigationFrame;
        }

        public void NavigateToHome()
        {
            if (this.rootNavigationFrame == null)
            {
                return;
            }

            this.rootNavigationFrame.Navigate(
                typeof(HomePage),
                serviceProvider.GetRequiredService<HomeViewModel>());
        }

        public void NavigateToConfiguredAirportRole()
        {
            if (this.rootNavigationFrame == null)
            {
                return;
            }

            MockUserRoleContext userRoleContext = mockUserUtility.GetRolesForUser(App.ConfiguredUserId);

            switch (userRoleContext.AirportRole)
            {
                case AirportModuleRole.None:
                    this.rootNavigationFrame.Navigate(typeof(NoAirportRolePage));
                    break;

                case AirportModuleRole.CompanyRepresentative:
                    this.NavigateToCompanyDashboard(userRoleContext.CompanyId ?? 1);
                    break;

                case AirportModuleRole.AirportAdministrator:
                    this.NavigateToAirportAdmin();
                    break;

                case AirportModuleRole.AirportStaffMember:
                    this.NavigateToStaffDashboard(userRoleContext.EmployeeId ?? App.ConfiguredUserId);
                    break;

                default:
                    this.NavigateToHome();
                    break;
            }
        }

        public void NavigateToConfiguredDutyFreeRole()
        {
            if (this.rootNavigationFrame == null)
            {
                return;
            }

            MockUserRoleContext userRoleContext = mockUserUtility.GetRolesForUser(App.ConfiguredUserId);

            if (userRoleContext.DutyFreeRole == DutyFreeModuleRole.Manager)
            {
                userSession.SetAdmin(userRoleContext.DutyFreeUserId);
            }
            else
            {
                userSession.SetClient(userRoleContext.DutyFreeUserId);
            }

            this.rootNavigationFrame.Navigate(typeof(ShopPage));
        }

        public void NavigateToSelectCompany()
        {
            if (this.rootNavigationFrame == null)
            {
                return;
            }

            this.rootNavigationFrame.Navigate(
                typeof(SelectCompanyPage),
                serviceProvider.GetRequiredService<SelectCompanyViewModel>());
        }

        public void NavigateToCompanyDashboard(int companyId)
        {
            if (this.rootNavigationFrame == null)
            {
                return;
            }

            this.rootNavigationFrame.Navigate(
                typeof(CompanyPage),
                (serviceProvider.GetRequiredService<CompanyViewModel>(), companyId));
        }

        public void NavigateToStaffLogin()
        {
            if (this.rootNavigationFrame == null)
            {
                return;
            }

            MockUserRoleContext userRoleContext = mockUserUtility.GetRolesForUser(App.ConfiguredUserId);

            int targetEmployeeId = userRoleContext.EmployeeId ?? App.ConfiguredUserId;

            this.NavigateToStaffDashboard(targetEmployeeId);
        }

        public void NavigateToAirportAdmin()
        {
            if (this.rootNavigationFrame == null)
            {
                return;
            }

            var adminViewModel = serviceProvider.GetRequiredService<AirportAdminViewModel>();
            var flightsViewModel = serviceProvider.GetRequiredService<FlightsDashboardViewModel>();
            var employeesViewModel = serviceProvider.GetRequiredService<EmployeesDashboardViewModel>();
            var dashboardViewModel = serviceProvider.GetRequiredService<AirportDashboardViewModel>();

            this.rootNavigationFrame.Navigate(
                typeof(AirportAdminPage),
                (adminViewModel, flightsViewModel, employeesViewModel, dashboardViewModel));
        }

        public void NavigateToStaffDashboard(int employeeId)
        {
            if (this.rootNavigationFrame == null)
            {
                return;
            }

            this.rootNavigationFrame.Navigate(
                typeof(StaffPage),
                (serviceProvider.GetRequiredService<StaffPageViewModel>(), employeeId));
        }
    }
}

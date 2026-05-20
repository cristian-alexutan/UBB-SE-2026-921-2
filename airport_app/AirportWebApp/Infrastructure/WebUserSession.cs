namespace AirportWebApp.Infrastructure
{
    public class WebUserSession
    {
        private readonly MockUserRoles roles;

        public WebUserSession(IConfiguration configuration)
        {
            int userId = configuration.GetValue<int>("UserID");
            roles = new MockUserUtil().GetRolesForUser(userId);
        }

        public int UserId => roles.UserId;

        public AirportModuleRole AirportRole => roles.AirportRole;

        public DutyFreeModuleRole DutyFreeRole => roles.DutyFreeRole;

        public int? CompanyId => roles.CompanyId;

        public int? EmployeeId => roles.EmployeeId;

        public int DutyFreeUserId => roles.DutyFreeUserId;

        public bool IsAirportAdmin => roles.AirportRole == AirportModuleRole.AirportAdministrator;

        public bool IsCompanyRepresentative => roles.AirportRole == AirportModuleRole.CompanyRepresentative;

        public bool IsAirportStaffMember => roles.AirportRole == AirportModuleRole.AirportStaffMember;

        public bool HasNoAirportRole => roles.AirportRole == AirportModuleRole.None;

        public bool IsDutyFreeManager => roles.DutyFreeRole == DutyFreeModuleRole.Manager;

        public bool IsDutyFreeClient => roles.DutyFreeRole == DutyFreeModuleRole.Client;
    }
}

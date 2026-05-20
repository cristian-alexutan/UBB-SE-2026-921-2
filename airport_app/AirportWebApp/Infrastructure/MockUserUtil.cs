namespace AirportWebApp.Infrastructure
{
    public enum AirportModuleRole
    {
        None,
        CompanyRepresentative,
        AirportAdministrator,
        AirportStaffMember,
    }

    public enum DutyFreeModuleRole
    {
        Client,
        Manager,
    }

    public sealed record MockUserRoles(
        int UserId,
        AirportModuleRole AirportRole,
        int? CompanyId,
        int? EmployeeId,
        DutyFreeModuleRole DutyFreeRole,
        int DutyFreeUserId);

    public class MockUserUtil
    {
        public MockUserRoles GetRolesForUser(int userId)
        {
            return userId switch
            {
                // Seeded employee 1, seeded client 1.
                1 => new MockUserRoles(userId, AirportModuleRole.AirportStaffMember, null, 1, DutyFreeModuleRole.Client, 1),

                // Seeded company 1, seeded manager 1.
                2 => new MockUserRoles(userId, AirportModuleRole.CompanyRepresentative, 1, null, DutyFreeModuleRole.Manager, 1),

                // Airport administrator has no backing table in the seeded data.
                3 => new MockUserRoles(userId, AirportModuleRole.AirportAdministrator, null, null, DutyFreeModuleRole.Client, 1),

                // Seeded employee 4, seeded client 1.
                4 => new MockUserRoles(userId, AirportModuleRole.AirportStaffMember, null, 4, DutyFreeModuleRole.Client, 1),

                // Duty Free-only user backed by seeded client 1.
                5 => new MockUserRoles(userId, AirportModuleRole.None, null, null, DutyFreeModuleRole.Client, 1),

                _ => new MockUserRoles(userId, AirportModuleRole.AirportStaffMember, null, 1, DutyFreeModuleRole.Client, 1),
            };
        }
    }
}

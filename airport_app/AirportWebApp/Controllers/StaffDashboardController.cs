using AirportWebApp.Infrastructure;
using AirportWebApp.Models.AirportManagement;
using Microsoft.AspNetCore.Mvc;

namespace AirportWebApp.Controllers;

[RequireAirportRole(AirportModuleRole.AirportStaffMember)]
public class StaffDashboardController : Controller
{
    private readonly WebUserSession session;
    private readonly IEmployeeService employeeService;
    private readonly IEmployeeFlightService employeeFlightService;

    public StaffDashboardController(
        WebUserSession session,
        IEmployeeService employeeService,
        IEmployeeFlightService employeeFlightService)
    {
        this.session = session;
        this.employeeService = employeeService;
        this.employeeFlightService = employeeFlightService;
    }

    public IActionResult Index()
    {
        int employeeId = session.EmployeeId ?? 0;
        var employee = employeeService.GetEmployeeById(employeeId);
        var schedule = employeeFlightService.GetFormattedEmployeeSchedule(employeeId);

        var model = new StaffDashboardViewModel
        {
            EmployeeId = employeeId,
            EmployeeName = employee?.Name ?? string.Empty,
            Schedule = schedule,
        };

        return View(model);
    }
}

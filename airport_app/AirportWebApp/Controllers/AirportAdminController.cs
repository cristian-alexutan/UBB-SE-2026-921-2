using AirportWebApp.Infrastructure;
using AirportWebApp.Models.AirportManagement;
using Microsoft.AspNetCore.Mvc;

namespace AirportWebApp.Controllers;

[RequireAirportRole(AirportModuleRole.AirportAdministrator)]
public class AirportAdminController : Controller
{
    private readonly WebUserSession session;
    private readonly IFlightRouteService flightRouteService;
    private readonly IFlightService flightService;
    private readonly IEmployeeService employeeService;
    private readonly IEmployeeFlightService employeeFlightService;
    private readonly IAirportService airportService;
    private readonly IRunwayService runwayService;
    private readonly IGateService gateService;
    private readonly ICompanyService companyService;

    public AirportAdminController(
        WebUserSession session,
        IFlightRouteService flightRouteService,
        IFlightService flightService,
        IEmployeeService employeeService,
        IEmployeeFlightService employeeFlightService,
        IAirportService airportService,
        IRunwayService runwayService,
        IGateService gateService,
        ICompanyService companyService)
    {
        this.session = session;
        this.flightRouteService = flightRouteService;
        this.flightService = flightService;
        this.employeeService = employeeService;
        this.employeeFlightService = employeeFlightService;
        this.airportService = airportService;
        this.runwayService = runwayService;
        this.gateService = gateService;
        this.companyService = companyService;
    }

    public IActionResult Index() => RedirectToAction(nameof(Flights));

    public IActionResult Flights(string? search)
    {
        var allFlights = flightRouteService.GetAllFlightsWithDetails();
        if (!string.IsNullOrWhiteSpace(search))
        {
            allFlights = flightRouteService.SearchFlights(allFlights, search);
        }

        var summaries = allFlights
            .Select(f => flightRouteService.BuildFlightSummary(f, employeeFlightService.FormatCrewList(f.Id)))
            .ToList();

        var model = new FlightsDashboardViewModel
        {
            Flights = summaries,
            SearchQuery = search ?? string.Empty,
        };

        return View(model);
    }

    public IActionResult AddFlightForm()
    {
        var form = BuildAddFlightForm(companyId: 0);
        return View(form);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddFlight(AddFlightFormModel form)
    {
        form.FlightNumberPrefix = companyService.GenerateFlightCodeUsingCompanyId(form.CompanyId);
        ModelState.Remove(nameof(AddFlightFormModel.FlightNumberPrefix));

        if (!ModelState.IsValid)
        {
            form.Airports = airportService.GetAllAirports();
            form.Runways = runwayService.GetAllRunways();
            form.Gates = gateService.GetAllGates();
            return View(nameof(AddFlightForm), form);
        }

        flightRouteService.CreateFlightWithSchedule(
            form.CompanyId,
            form.RouteType,
            form.AirportId,
            form.Capacity,
            TimeSpan.FromMinutes(form.DepartureOffsetMinutes),
            TimeSpan.FromMinutes(form.ArrivalOffsetMinutes),
            form.IsRecurrent,
            form.StartDate,
            form.EndDate,
            form.SingleDate,
            form.RecurrenceType,
            form.CustomDaysText,
            form.RunwayId,
            form.GateId,
            _ => form.FlightNumberPrefix);

        return RedirectToAction(nameof(Flights));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteFlight(int id)
    {
        flightRouteService.DeleteFlightUsingId(id);
        return RedirectToAction(nameof(Flights));
    }

    public IActionResult ManageCrew(int flightId)
    {
        var flight = flightRouteService.GetFlightById(flightId);
        if (flight == null)
        {
            return NotFound();
        }

        var crewData = employeeFlightService.GetCrewSelectionDataById(flightId);
        var crewText = employeeFlightService.FormatCrewList(flightId);
        var summary = flightRouteService.BuildFlightSummary(flight, crewText);

        var model = new CrewManagementViewModel
        {
            FlightId = flightId,
            Flight = summary,
            CrewCandidates = crewData,
            SelectedEmployeeIds = employeeFlightService
                .GetEmployeesAssignedToFlight(flightId)
                .Select(e => e.Id)
                .ToList(),
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SaveCrew(int flightId, List<int> selectedEmployeeIds)
    {
        employeeFlightService.UpdateEmployeesForFlightUsingIds(flightId, selectedEmployeeIds ?? new List<int>());
        return RedirectToAction(nameof(Flights));
    }

    public IActionResult Employees()
    {
        var model = new EmployeesDashboardViewModel
        {
            Employees = employeeService.GetAllEmployees(),
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddEmployee(EmployeeFormModel form)
    {
        if (ModelState.IsValid)
        {
            employeeService.AddEmployee(
                form.Name,
                employeeService.ParseRole(form.Role),
                DateOnly.FromDateTime(DateTime.Today),
                0,
                DateOnly.FromDateTime(DateTime.Today));
        }

        return RedirectToAction(nameof(Employees));
    }

    public IActionResult EditEmployeeForm(int id)
    {
        var employee = employeeService.GetEmployeeById(id);
        if (employee == null)
        {
            return NotFound();
        }

        var form = new EmployeeFormModel
        {
            Id = employee.Id,
            Name = employee.Name,
            Role = employee.Role.ToString(),
        };

        return View(form);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditEmployee(EmployeeFormModel form)
    {
        if (ModelState.IsValid)
        {
            employeeService.UpdateEmployee(
                form.Id,
                name: form.Name,
                role: employeeService.ParseRole(form.Role));
        }

        return RedirectToAction(nameof(Employees));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteEmployee(int id)
    {
        employeeService.DeleteWithAssignments(id);
        return RedirectToAction(nameof(Employees));
    }

    public IActionResult Configuration()
    {
        return View();
    }

    private AddFlightFormModel BuildAddFlightForm(int companyId)
    {
        return new AddFlightFormModel
        {
            CompanyId = companyId,
            Airports = airportService.GetAllAirports(),
            Runways = runwayService.GetAllRunways(),
            Gates = gateService.GetAllGates(),
        };
    }
}

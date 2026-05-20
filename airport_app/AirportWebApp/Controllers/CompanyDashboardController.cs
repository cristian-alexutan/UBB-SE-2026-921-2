using AirportWebApp.Infrastructure;
using AirportWebApp.Models.AirportManagement;
using Microsoft.AspNetCore.Mvc;

namespace AirportWebApp.Controllers;

public class CompanyDashboardController : Controller
{
    private readonly WebUserSession session;
    private readonly IFlightRouteService flightRouteService;
    private readonly IFlightService flightService;
    private readonly ICompanyService companyService;
    private readonly IEmployeeFlightService employeeFlightService;
    private readonly IAirportService airportService;
    private readonly IRunwayService runwayService;
    private readonly IGateService gateService;

    public CompanyDashboardController(
        WebUserSession session,
        IFlightRouteService flightRouteService,
        IFlightService flightService,
        ICompanyService companyService,
        IEmployeeFlightService employeeFlightService,
        IAirportService airportService,
        IRunwayService runwayService,
        IGateService gateService)
    {
        this.session = session;
        this.flightRouteService = flightRouteService;
        this.flightService = flightService;
        this.companyService = companyService;
        this.employeeFlightService = employeeFlightService;
        this.airportService = airportService;
        this.runwayService = runwayService;
        this.gateService = gateService;
    }

    public IActionResult Index(string? search)
    {
        int companyId = session.CompanyId ?? 0;
        var company = companyService.GetCompanyById(companyId);

        var allFlights = flightRouteService.GetFlightsByCompanyId(companyId);
        if (!string.IsNullOrWhiteSpace(search))
        {
            allFlights = flightRouteService.SearchFlights(allFlights, search);
        }

        var summaries = allFlights
            .Select(f => flightRouteService.BuildFlightSummary(f, employeeFlightService.FormatCrewList(f.Id)))
            .ToList();

        var model = new CompanyDashboardViewModel
        {
            CompanyId = companyId,
            CompanyName = company?.Name ?? string.Empty,
            Flights = summaries,
            SearchQuery = search ?? string.Empty,
            AddFlightForm = BuildAddFlightForm(companyId),
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddFlight(AddFlightFormModel form)
    {
        if (ModelState.IsValid)
        {
            flightRouteService.CreateFlightWithSchedule(
                form.CompanyId,
                form.RouteType,
                form.AirportId,
                form.Capacity,
                form.DepartureOffset,
                form.ArrivalOffset,
                form.IsRecurrent,
                form.StartDate,
                form.EndDate,
                form.SingleDate,
                form.RecurrenceType,
                form.CustomDaysText,
                form.RunwayId,
                form.GateId,
                _ => form.FlightNumberPrefix);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteFlight(int id)
    {
        flightRouteService.DeleteFlightUsingId(id);
        return RedirectToAction(nameof(Index));
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
        return RedirectToAction(nameof(Index));
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

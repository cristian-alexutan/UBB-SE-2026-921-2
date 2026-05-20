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

    [HttpGet("/CompanyRepresentative")]
    [HttpGet("/CompanyDashboard")]
    public IActionResult Index(string? search)
    {
        return View(BuildDashboardModel(session.CompanyId ?? 0, search));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddFlight(AddFlightFormModel form)
    {
        try
        {
            form.FlightNumberPrefix = companyService.GenerateFlightCodeUsingCompanyId(form.CompanyId);
            ModelState.Remove(nameof(AddFlightFormModel.FlightNumberPrefix));

            if (!ModelState.IsValid)
            {
                PopulateAddFlightDropdowns(form);
                return View(nameof(Index), BuildDashboardModel(form.CompanyId, null, form, true));
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

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            PopulateAddFlightDropdowns(form);
            ModelState.AddModelError(string.Empty, ex.GetBaseException().Message);
            return View(nameof(Index), BuildDashboardModel(form.CompanyId, null, form, true));
        }
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

    private CompanyDashboardViewModel BuildDashboardModel(int companyId, string? search, AddFlightFormModel? addFlightForm = null, bool showAddFlightForm = false)
    {
        var company = companyService.GetCompanyById(companyId);
        var allFlights = flightRouteService.GetFlightsByCompanyId(companyId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            allFlights = flightRouteService.SearchFlights(allFlights, search);
        }

        var summaries = allFlights
            .Select(f => flightRouteService.BuildFlightSummary(f, employeeFlightService.FormatCrewList(f.Id)))
            .ToList();

        return new CompanyDashboardViewModel
        {
            CompanyId = companyId,
            CompanyName = company?.Name ?? string.Empty,
            Flights = summaries,
            SearchQuery = search ?? string.Empty,
            AddFlightForm = addFlightForm ?? BuildAddFlightForm(companyId),
            ShowAddFlightForm = showAddFlightForm,
        };
    }

    private void PopulateAddFlightDropdowns(AddFlightFormModel form)
    {
        form.Airports = airportService.GetAllAirports();
        form.Runways = runwayService.GetAllRunways();
        form.Gates = gateService.GetAllGates();
    }

    private AddFlightFormModel BuildAddFlightForm(int companyId)
    {
        var form = new AddFlightFormModel
        {
            CompanyId = companyId,
        };

        PopulateAddFlightDropdowns(form);
        return form;
    }
}

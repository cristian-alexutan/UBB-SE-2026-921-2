using Microsoft.AspNetCore.Mvc;

using AirportLib.Domain.User;

namespace AirportWebApp.Controllers;

[RequireAirportRole(AirportModuleRole.CompanyRepresentative)]
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
    public IActionResult AddFlight([Bind(Prefix = "AddFlightForm")] AddFlightFormModel form)
    {
        try
        {
            form.CompanyId = session.CompanyId ?? 0;

            TimeSpan departureOffset = CalculateOffset(form.DepartureHour, form.DepartureHour, form.DepartureAmPm);
            TimeSpan arrivalOffset = CalculateOffset(form.ArrivalHour, form.ArrivalMinute, form.ArrivalAmPm);

            ModelState.Clear();
            TryValidateModel(form);

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
                departureOffset,
                arrivalOffset,
                form.IsRecurrent,
                form.StartDate,
                form.EndDate,
                form.SingleDate,
                form.RecurrenceType,
                form.CustomDaysText,
                form.RunwayId,
                form.GateId,
                _ => companyService.GenerateFlightCodeUsingCompanyId(form.CompanyId));

            return RedirectToAction(nameof(Index));
        }
        catch (Exception exception)
        {
            PopulateAddFlightDropdowns(form);
            ModelState.AddModelError(string.Empty, exception.GetBaseException().Message);
            return View(nameof(Index), BuildDashboardModel(form.CompanyId, null, form, true));
        }
    }

    private TimeSpan CalculateOffset(int hour, int minute, string amPm)
    {
        int militaryHour = hour % 12;
        if (string.Equals(amPm, "PM", StringComparison.OrdinalIgnoreCase))
        {
            militaryHour += 12;
        }
        return new TimeSpan(militaryHour, minute, 0);
    }

    [HttpGet]
    public IActionResult DeleteFlight(int flightId)
    {
        if (flightId <= 0)
        {
            return this.NotFound();
        }

        var flightInstance = flightRouteService.GetFlightById(flightId);

        if (flightInstance == null || !this.CanAccessFlight(flightInstance))
        {
            return this.NotFound("Flight not found or access denied.");
        }

        string crewListText = employeeFlightService.FormatCrewList(flightId);
        var viewModel = flightRouteService.BuildFlightSummary(flightInstance, crewListText);

        return this.View(viewModel);
    }

    [HttpPost]
    [ActionName("DeleteFlight")]
    [ValidateAntiForgeryToken]
    public IActionResult ExecuteDeleteFlight(int flightId)
    {
        if (!this.CanAccessFlight(flightId))
        {
            return this.Forbid();
        }

        employeeFlightService.RemoveAllCrewAssignmentsForFlight(flightId);
        flightRouteService.DeleteFlightUsingId(flightId);

        return this.RedirectToAction(nameof(this.Index));
    }

    public IActionResult ManageCrew(int flightId)
    {
        var flight = flightRouteService.GetFlightById(flightId);
        if (flight == null)
        {
            return NotFound();
        }

        if (!CanAccessFlight(flight))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
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
        if (!CanAccessFlight(flightId))
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        employeeFlightService.UpdateEmployeesForFlightUsingIds(flightId, selectedEmployeeIds ?? new List<int>());
        return RedirectToAction(nameof(Index));
    }

    private bool CanAccessFlight(int flightId)
    {
        var flight = flightRouteService.GetFlightById(flightId);
        return flight != null && CanAccessFlight(flight);
    }

    private bool CanAccessFlight(Flight flight)
    {
        return session.CompanyId.HasValue && flight.Route?.Company?.Id == session.CompanyId.Value;
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

using Microsoft.AspNetCore.Mvc;

namespace AirportWebApp.Controllers;

public class AirportAdministrationController(
    WebUserSession userSession,
    IFlightRouteService flightRouteService,
    IFlightService flightService,
    IEmployeeService employeeService,
    IEmployeeFlightService employeeFlightService,
    IAirportService airportService,
    IRunwayService runwayService,
    IGateService gateService) : Controller
{
    public IActionResult Index()
    {
        return this.RedirectToAction(nameof(this.DisplayFlights));
    }

    [HttpGet]
    public IActionResult DisplayFlights(string? search)
    {
        List<Flight> allFlightsList = flightRouteService.GetAllFlightsWithDetails();

        if (!string.IsNullOrWhiteSpace(search))
        {
            allFlightsList = flightRouteService.SearchFlights(allFlightsList, search);
        }

        List<FlightSummary> flightSummaries = new List<FlightSummary>();
        foreach (Flight flightInstance in allFlightsList)
        {
            string formattedCrewText = employeeFlightService.FormatCrewList(flightInstance.Id);
            flightSummaries.Add(flightRouteService.BuildFlightSummary(flightInstance, formattedCrewText));
        }

        FlightsDashboardViewModel viewModel = new FlightsDashboardViewModel
        {
            Flights = flightSummaries,
            SearchQuery = search ?? string.Empty
        };

        return this.View(viewModel);
    }

    [HttpGet]
    public IActionResult OpenAddFlightForm()
    {
        const int NewFlightCompanyContext = 0;
        AddFlightFormModel formModel = this.CreateAddFlightFormViewModel(NewFlightCompanyContext);

        return this.View(formModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExecuteAddFlight(AddFlightFormModel form)
    {
        if (!this.ModelState.IsValid)
        {
            form.Airports = airportService.GetAllAirports();
            form.Runways = runwayService.GetAllRunways();
            form.Gates = gateService.GetAllGates();
            return this.View(nameof(this.OpenAddFlightForm), form);
        }

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

        return this.RedirectToAction(nameof(this.DisplayFlights));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveFlight(int flightId)
    {
        if (flightId <= 0)
        {
            return this.BadRequest();
        }

        flightRouteService.DeleteFlightUsingId(flightId);
        return this.RedirectToAction(nameof(this.DisplayFlights));
    }

    [HttpGet]
    public IActionResult OpenCrewManagement(int flightId)
    {
        Flight? flightInstance = flightRouteService.GetFlightById(flightId);

        if (flightInstance == null)
        {
            return this.NotFound();
        }

        string crewListText = employeeFlightService.FormatCrewList(flightId);

        List<int> assignedEmployeeIds = new List<int>();
        List<Employee> assignedEmployees = employeeFlightService.GetEmployeesAssignedToFlight(flightId);

        foreach (Employee employee in assignedEmployees)
        {
            assignedEmployeeIds.Add(employee.Id);
        }

        CrewManagementViewModel viewModel = new CrewManagementViewModel
        {
            FlightId = flightId,
            Flight = flightRouteService.BuildFlightSummary(flightInstance, crewListText),
            CrewCandidates = employeeFlightService.GetCrewSelectionDataById(flightId),
            SelectedEmployeeIds = assignedEmployeeIds
        };

        return this.View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SaveCrewAssignment(int flightId, List<int> selectedEmployeeIds)
    {
        List<int> validEmployeeIds = selectedEmployeeIds ?? new List<int>();
        employeeFlightService.UpdateEmployeesForFlightUsingIds(flightId, validEmployeeIds);

        return this.RedirectToAction(nameof(this.DisplayFlights));
    }

    [HttpGet]
    public IActionResult DisplayEmployees()
    {
        EmployeesDashboardViewModel viewModel = new EmployeesDashboardViewModel
        {
            Employees = employeeService.GetAllEmployees()
        };
        return this.View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExecuteAddEmployee(EmployeeFormModel form)
    {
        if (this.ModelState.IsValid)
        {
            employeeService.AddEmployee(
                form.Name,
                employeeService.ParseRole(form.Role),
                DateOnly.FromDateTime(DateTime.Today),
                0,
                DateOnly.FromDateTime(DateTime.Today));
        }

        return this.RedirectToAction(nameof(this.DisplayEmployees));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ExecuteRemoveEmployee(int employeeId)
    {
        employeeService.DeleteWithAssignments(employeeId);
        return this.RedirectToAction(nameof(this.DisplayEmployees));
    }

    [HttpGet]
    public IActionResult DisplayConfiguration()
    {
        AirportAdminViewModel viewModel = new AirportAdminViewModel
        {
            RunwaysList = runwayService.GetAllRunways(),
            GatesList = gateService.GetAllGates(),
            AirportsList = airportService.GetAllAirports()
        };

        return this.View(viewModel);
    }

    private AddFlightFormModel CreateAddFlightFormViewModel(int companyId)
    {
        return new AddFlightFormModel
        {
            CompanyId = companyId,
            Airports = airportService.GetAllAirports(),
            Runways = runwayService.GetAllRunways(),
            Gates = gateService.GetAllGates()
        };
    }
}
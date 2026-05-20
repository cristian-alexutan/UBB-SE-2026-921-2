using System.Diagnostics;

using AirportWebApp.Models;

using Microsoft.AspNetCore.Mvc;

namespace AirportWebApp.Controllers;

public class HomeController(WebUserSession userSession) : Controller
{
    private const string AdministrationController = "AirportAdministration";
    private const string AdministrationAction = "DisplayFlights";

    private const string CompanyController = "CompanyDashboard";
    private const string StaffController = "StaffDashboard";
    private const string DefaultAction = "Index";


    public IActionResult Index()
    {
        switch (userSession.AirportRole)
        {
            case AirportModuleRole.AirportAdministrator:
                return this.RedirectToAction(AdministrationAction, AdministrationController);

            case AirportModuleRole.CompanyRepresentative:
                return this.RedirectToAction(DefaultAction, CompanyController);

            case AirportModuleRole.AirportStaffMember:
                return this.RedirectToAction(DefaultAction, StaffController);

            default:
                return this.View("NoRole", userSession);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

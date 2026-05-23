using Microsoft.AspNetCore.Mvc;
using AirportLib.Domain.User;

namespace AirportWebApp.Controllers;

public class AirportManagementController : Controller
{
    private readonly WebUserSession session;

    public AirportManagementController(WebUserSession session)
    {
        this.session = session;
    }

    public IActionResult Index()
    {
        return session.AirportRole switch
        {
            AirportModuleRole.AirportAdministrator => RedirectToAction("DisplayFlights", "AirportAdministration"),
            AirportModuleRole.CompanyRepresentative => RedirectToAction("Index", "CompanyDashboard"),
            AirportModuleRole.AirportStaffMember => RedirectToAction("Index", "StaffDashboard"),
            _ => View("~/Views/Home/NoRole.cshtml", session),
        };
    }
}

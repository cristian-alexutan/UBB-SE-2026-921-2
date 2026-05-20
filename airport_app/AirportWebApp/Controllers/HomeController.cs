using System.Diagnostics;
using AirportWebApp.Infrastructure;
using AirportWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirportWebApp.Controllers;

public class HomeController : Controller
{
    private readonly WebUserSession session;

    public HomeController(WebUserSession session)
    {
        this.session = session;
    }

    public IActionResult Index()
    {
        return View(session);
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

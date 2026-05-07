using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/airports")]
public class AirportsController : ControllerBase
{
    private readonly IAirportRepository airportRepository;

    public AirportsController(IAirportRepository airportRepository)
    {
        this.airportRepository = airportRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Airport>> GetAll()
    {
        return Ok(airportRepository.GetAllAirports());
    }

    [HttpGet("{airportId:int}")]
    public ActionResult<Airport> GetById(int airportId)
    {
        Airport? airport = airportRepository.GetAirportById(airportId);
        return airport == null ? NotFound() : Ok(airport);
    }

    [HttpPost]
    public ActionResult<Airport> Add(Airport airport)
    {
        int airportId = airportRepository.AddAirport(airport);
        return CreatedAtAction(nameof(GetById), new { airportId }, airport);
    }

    [HttpPut("{airportId:int}")]
    public IActionResult Update(int airportId, Airport airport)
    {
        if (airportRepository.GetAirportById(airportId) == null)
        {
            return NotFound();
        }

        airport.Id = airportId;
        airportRepository.UpdateAirport(airport);
        return NoContent();
    }

    [HttpDelete("{airportId:int}")]
    public IActionResult Delete(int airportId)
    {
        if (airportRepository.GetAirportById(airportId) == null)
        {
            return NotFound();
        }

        airportRepository.DeleteAirportUsingId(airportId);
        return NoContent();
    }
}

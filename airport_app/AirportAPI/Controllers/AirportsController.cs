using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportsController(IAirportRepository airportRepository) : ControllerBase
{
    private const string MissingDataErrorMessage = "Airport data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Airport>> GetAll()
    {
        return this.Ok(airportRepository.GetAllAirports());
    }

    [HttpGet("{airportId:int}")]
    public ActionResult<Airport> GetById(int airportId)
    {
        Airport? airport = airportRepository.GetAirportById(airportId);

        if (airport == null)
        {
            return this.NotFound();
        }

        return this.Ok(airport);
    }

    [HttpPost]
    public ActionResult<Airport> Add([FromBody] Airport airport)
    {
        if (airport == null)
        {
            return this.BadRequest(MissingDataErrorMessage);
        }

        int airportId = airportRepository.AddAirport(airport);

        return this.CreatedAtAction(nameof(this.GetById), new { airportId }, airport);
    }
    [HttpPut("{airportId:int}")]
    public IActionResult Update(int airportId, [FromBody] Airport airport)
    {
        if (airportRepository.GetAirportById(airportId) == null)
        {
            return this.NotFound();
        }

        if (airport == null)
        {
            return this.BadRequest(MissingDataErrorMessage);
        }

        airport.Id = airportId;
        airportRepository.UpdateAirport(airport);

        return this.NoContent();
    }

    [HttpDelete("{airportId:int}")]
    public IActionResult Delete(int airportId)
    {
        if (airportRepository.GetAirportById(airportId) == null)
        {
            return this.NotFound();
        }

        airportRepository.DeleteAirportUsingId(airportId);

        return this.NoContent();
    }
}
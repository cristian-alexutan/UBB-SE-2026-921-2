using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/runways")]
public class RunwaysController : ControllerBase
{
    private readonly IRunwayRepository runwayRepository;

    public RunwaysController(IRunwayRepository runwayRepository)
    {
        this.runwayRepository = runwayRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Runway>> GetAll()
    {
        return Ok(runwayRepository.GetAllRunways());
    }

    [HttpGet("{runwayId:int}")]
    public ActionResult<Runway> GetById(int runwayId)
    {
        Runway? runway = runwayRepository.GetRunwayById(runwayId);
        return runway == null ? NotFound() : Ok(runway);
    }

    [HttpPost]
    public ActionResult<Runway> Add(Runway runway)
    {
        int runwayId = runwayRepository.AddRunway(runway);
        return CreatedAtAction(nameof(GetById), new { runwayId }, runway);
    }

    [HttpPut("{runwayId:int}")]
    public IActionResult Update(int runwayId, Runway runway)
    {
        if (runwayRepository.GetRunwayById(runwayId) == null)
        {
            return NotFound();
        }

        runway.Id = runwayId;
        runwayRepository.UpdateRunway(runway);
        return NoContent();
    }

    [HttpDelete("{runwayId:int}")]
    public IActionResult Delete(int runwayId)
    {
        if (runwayRepository.GetRunwayById(runwayId) == null)
        {
            return NotFound();
        }

        runwayRepository.DeleteRunwayUsingId(runwayId);
        return NoContent();
    }
}

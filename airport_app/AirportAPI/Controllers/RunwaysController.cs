using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RunwaysController(IRunwayRepository runwayRepository) : ControllerBase
{
    private const string NullRunwayDataErrorMessage = "Runway data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Runway>> GetAll()
    {
        return this.Ok(runwayRepository.GetAllRunways());
    }

    [HttpGet("{runwayId:int}")]
    public ActionResult<Runway> GetById(int runwayId)
    {
        Runway? runway = runwayRepository.GetRunwayById(runwayId);

        if (runway == null)
        {
            return this.NotFound();
        }

        return this.Ok(runway);
    }

    [HttpPost]
    public ActionResult<Runway> Add([FromBody] Runway runway)
    {
        if (runway == null)
        {
            return this.BadRequest(NullRunwayDataErrorMessage);
        }

        int runwayId = runwayRepository.AddRunway(runway);

        return this.CreatedAtAction(nameof(this.GetById), new { runwayId }, runway);
    }

    [HttpPut("{runwayId:int}")]
    public IActionResult Update(int runwayId, [FromBody] Runway runway)
    {
        if (runway == null)
        {
            return this.BadRequest(NullRunwayDataErrorMessage);
        }

        if (runwayRepository.GetRunwayById(runwayId) == null)
        {
            return this.NotFound();
        }

        runway.Id = runwayId;

        runwayRepository.UpdateRunway(runway);

        return this.NoContent();
    }

    [HttpDelete("{runwayId:int}")]
    public IActionResult Delete(int runwayId)
    {
        if (runwayRepository.GetRunwayById(runwayId) == null)
        {
            return this.NotFound();
        }

        runwayRepository.DeleteRunwayUsingId(runwayId);

        return this.NoContent();
    }
}
using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/runways")]
public class RunwaysController(IRunwayService runwayService) : ControllerBase
{
    private const string EmptyRunwayNameErrorMessage = "The runway name cannot be empty.";
    private const string InvalidHandleTimeErrorMessage = "Handle time must be a valid positive numeric value.";

    [HttpGet]
    public ActionResult<IEnumerable<Runway>> GetAll()
    {
        return this.Ok(runwayService.GetAllRunways());
    }

    [HttpGet("{runwayId:int}")]
    public ActionResult<Runway> GetById(int runwayId)
    {
        Runway? runway = runwayService.GetRunwayById(runwayId);

        if (runway == null)
        {
            return NotFound();
        }

        return this.Ok(runway);
    }

    [HttpPost]
    public ActionResult<int> Add([FromBody] RunwayDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return this.BadRequest(EmptyRunwayNameErrorMessage);
        }

        if (dto.HandleTime <= 0)
        {
            return this.BadRequest(InvalidHandleTimeErrorMessage);
        }

        int runwayId = runwayService.AddRunway(dto.Name, dto.HandleTime ?? 0);

        return this.Ok(runwayId);
    }

    [HttpPut("{runwayId:int}")]
    public ActionResult Update(int runwayId, [FromBody] RunwayDto dto)
    {
        if (runwayService.GetRunwayById(runwayId) == null)
        {
            return NotFound();
        }

        if (dto.Name != null && string.IsNullOrWhiteSpace(dto.Name))
        {
            return this.BadRequest(EmptyRunwayNameErrorMessage);
        }

        if (dto.HandleTime != null && dto.HandleTime <= 0)
        {
            return this.BadRequest(InvalidHandleTimeErrorMessage);
        }

        runwayService.UpdateRunway(runwayId, dto.Name, dto.HandleTime);

        return this.NoContent();
    }

    [HttpDelete("{runwayId:int}")]
    public ActionResult Delete(int runwayId)
    {
        if (runwayService.GetRunwayById(runwayId) == null)
        {
            return NotFound();
        }

        runwayService.DeleteRunwayUsingId(runwayId);

        return NoContent();
    }

    [HttpGet("{runwayId:int}/has-flights")]
    public ActionResult<bool> HasFlights(int runwayId)
    {
        if (runwayService.GetRunwayById(runwayId) == null)
        {
            return NotFound();
        }

        return this.Ok(runwayService.HasFlights(runwayId));
    }

    [HttpGet("{runwayId:int}/delete-warning")]
    public ActionResult<string> GetDeleteWarningMessage(int runwayId)
    {
        if (runwayService.GetRunwayById(runwayId) == null)
        {
            return NotFound();
        }

        string deleteWarning = runwayService.GetDeleteWarningMessage(runwayId);

        return this.Ok(new { message = deleteWarning });
    }

    public sealed class RunwayDto
    {
        public string? Name { get; set; }

        public int? HandleTime { get; set; }
    }
}

using AirportAPI.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/gates")]
public class GatesController(IGateService gateService) : ControllerBase
{
    private const string EmptyGateNameErrorMessage = "The gate name cannot be empty.";

    [HttpGet]
    public ActionResult<IEnumerable<Gate>> GetAll()
    {
        return this.Ok(gateService.GetAllGates());
    }

    [HttpGet("{gateId:int}")]
    public ActionResult<Gate> GetById(int gateId)
    {
        Gate? gate = gateService.GetGateById(gateId);

        if (gate == null)
        {
            return NotFound();
        }

        return this.Ok(gate);
    }

    [HttpPost]
    public ActionResult<int> Add([FromBody] string gateName)
    {
        if (string.IsNullOrWhiteSpace(gateName))
        {
            return this.BadRequest(EmptyGateNameErrorMessage);
        }

        int gateId = gateService.AddGate(gateName);

        return this.Ok(gateId);
    }

    [HttpPut("{gateId:int}")]
    public ActionResult Update(int gateId, [FromBody] string updatedGateName)
    {
        if (gateService.GetGateById(gateId) == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(updatedGateName))
        {
            return this.BadRequest(EmptyGateNameErrorMessage);
        }

        gateService.UpdateGate(gateId, updatedGateName);

        return this.NoContent();
    }

    [HttpDelete("{gateId:int}")]
    public ActionResult Delete(int gateId)
    {
        if (gateService.GetGateById(gateId) == null)
        {
            return NotFound();
        }

        gateService.DeleteGateUsingId(gateId);

        return NoContent();
    }

    [HttpGet("{gateId:int}/has-flights")]
    public ActionResult<bool> HasFlights(int gateId)
    {
        if (gateService.GetGateById(gateId) == null)
        {
            return NotFound();
        }

        return this.Ok(gateService.HasFlights(gateId));
    }

    [HttpGet("{gateId:int}/delete-warning")]
    public ActionResult<string> GetDeleteWarningMessage(int gateId)
    {
        if (gateService.GetGateById(gateId) == null)
        {
            return NotFound();
        }

        string deleteWarning = gateService.GetDeleteWarningMessage(gateId);

        return this.Ok(new { message = deleteWarning });
    }
}
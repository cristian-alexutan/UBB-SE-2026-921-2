using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GatesController(IGateRepository gateRepository) : ControllerBase
{
    private const string NullGateDataErrorMessage = "Gate data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Gate>> GetAll()
    {
        return this.Ok(gateRepository.GetAllGates());
    }

    [HttpGet("{gateId:int}")]
    public ActionResult<Gate> GetById(int gateId)
    {
        Gate? gate = gateRepository.GetGateById(gateId);

        if (gate == null)
        {
            return this.NotFound();
        }

        return this.Ok(gate);
    }

    [HttpPost]
    public ActionResult<Gate> Add([FromBody] Gate gate)
    {
        if (gate == null)
        {
            return this.BadRequest(NullGateDataErrorMessage);
        }

        int gateId = gateRepository.AddGate(gate);

        return this.CreatedAtAction(nameof(this.GetById), new { gateId }, gate);
    }

    [HttpPut("{gateId:int}")]
    public IActionResult Update(int gateId, [FromBody] Gate gate)
    {
        if (gate == null)
        {
            return this.BadRequest(NullGateDataErrorMessage);
        }

        if (gateRepository.GetGateById(gateId) == null)
        {
            return this.NotFound();
        }

        gate.Id = gateId;

        gateRepository.UpdateGate(gate);

        return this.NoContent();
    }

    [HttpDelete("{gateId:int}")]
    public IActionResult Delete(int gateId)
    {
        if (gateRepository.GetGateById(gateId) == null)
        {
            return this.NotFound();
        }

        gateRepository.DeleteGateUsingId(gateId);

        return this.NoContent();
    }
}
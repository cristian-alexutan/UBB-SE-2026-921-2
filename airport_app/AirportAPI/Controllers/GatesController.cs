using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/gates")]
public class GatesController : ControllerBase
{
    private readonly IGateRepository gateRepository;

    public GatesController(IGateRepository gateRepository)
    {
        this.gateRepository = gateRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Gate>> GetAll()
    {
        return Ok(gateRepository.GetAllGates());
    }

    [HttpGet("{gateId:int}")]
    public ActionResult<Gate> GetById(int gateId)
    {
        Gate? gate = gateRepository.GetGateById(gateId);
        return gate == null ? NotFound() : Ok(gate);
    }

    [HttpPost]
    public ActionResult<Gate> Add(Gate gate)
    {
        int gateId = gateRepository.AddGate(gate);
        return CreatedAtAction(nameof(GetById), new { gateId }, gate);
    }

    [HttpPut("{gateId:int}")]
    public IActionResult Update(int gateId, Gate gate)
    {
        if (gateRepository.GetGateById(gateId) == null)
        {
            return NotFound();
        }

        gate.Id = gateId;
        gateRepository.UpdateGate(gate);
        return NoContent();
    }

    [HttpDelete("{gateId:int}")]
    public IActionResult Delete(int gateId)
    {
        if (gateRepository.GetGateById(gateId) == null)
        {
            return NotFound();
        }

        gateRepository.DeleteGateUsingId(gateId);
        return NoContent();
    }
}

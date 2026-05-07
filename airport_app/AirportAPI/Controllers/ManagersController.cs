using AirportAPI.Domain;
using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/managers")]
public class ManagersController : ControllerBase
{
    private readonly IManagerRepo managerRepo;

    public ManagersController(IManagerRepo managerRepo)
    {
        this.managerRepo = managerRepo;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Manager>> GetAll()
    {
        return Ok(managerRepo.GetAll());
    }

    [HttpGet("{managerId:int}")]
    public ActionResult<Manager> GetById(int managerId)
    {
        Manager? manager = managerRepo.GetById(managerId);
        return manager == null ? NotFound() : Ok(manager);
    }

    [HttpPost]
    public ActionResult<Manager> Add(Manager manager)
    {
        managerRepo.Add(manager);
        return CreatedAtAction(nameof(GetById), new { managerId = manager.Id }, manager);
    }

    [HttpPut("{managerId:int}")]
    public ActionResult<Manager> Update(int managerId, Manager manager)
    {
        if (managerRepo.GetById(managerId) == null)
        {
            return NotFound();
        }

        manager.Id = managerId;
        Manager? updatedManager = managerRepo.Update(manager);
        return updatedManager == null ? NotFound() : Ok(updatedManager);
    }

    [HttpDelete("{managerId:int}")]
    public ActionResult<Manager> Delete(int managerId)
    {
        Manager? deletedManager = managerRepo.Delete(managerId);
        return deletedManager == null ? NotFound() : Ok(deletedManager);
    }
}

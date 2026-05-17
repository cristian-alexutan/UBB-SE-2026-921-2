using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManagersController(IManagerRepository managerRepository) : ControllerBase
{
    private const string NullManagerDataErrorMessage = "Manager data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Manager>> GetAll()
    {
        return this.Ok(managerRepository.GetAll());
    }

    [HttpGet("{managerId:int}")]
    public ActionResult<Manager> GetById(int managerId)
    {
        Manager? manager = managerRepository.GetById(managerId);

        if (manager == null)
        {
            return this.NotFound();
        }

        return this.Ok(manager);
    }

    [HttpPost]
    public ActionResult<Manager> Add([FromBody] Manager manager)
    {
        if (manager == null)
        {
            return this.BadRequest(NullManagerDataErrorMessage);
        }

        managerRepository.Add(manager);

        return this.CreatedAtAction(nameof(this.GetById), new { managerId = manager.Id }, manager);
    }

    [HttpPut("{managerId:int}")]
    public ActionResult<Manager> Update(int managerId, [FromBody] Manager manager)
    {
        if (manager == null)
        {
            return this.BadRequest(NullManagerDataErrorMessage);
        }

        if (managerRepository.GetById(managerId) == null)
        {
            return this.NotFound();
        }

        manager.Id = managerId;

        Manager? updatedManager = managerRepository.Update(manager);

        if (updatedManager == null)
        {
            return this.NotFound();
        }

        return this.Ok(updatedManager);
    }

    [HttpDelete("{managerId:int}")]
    public ActionResult<Manager> Delete(int managerId)
    {
        Manager? deletedManager = managerRepository.Delete(managerId);

        if (deletedManager == null)
        {
            return this.NotFound();
        }

        return this.Ok(deletedManager);
    }
}
using AirportAPI.Domain;
using AirportAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/managers")]
public class ManagersController(IManagerService managerService) : ControllerBase
{
    private const string MissingManagerDataErrorMessage = "Manager data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Manager>> GetAll()
    {
        return this.Ok(managerService.GetAllManagers());
    }

    [HttpGet("{managerId:int}")]
    public ActionResult<Manager> GetById(int managerId)
    {
        Manager? manager = managerService.GetManagerById(managerId);

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
            return this.BadRequest(MissingManagerDataErrorMessage);
        }

        managerService.AddManager(manager);

        return this.CreatedAtAction(nameof(this.GetById), new { managerId = manager.Id }, manager);
    }

    [HttpPut("{managerId:int}")]
    public ActionResult<Manager> Update(int managerId, [FromBody] Manager manager)
    {
        if (manager == null)
        {
            return this.BadRequest(MissingManagerDataErrorMessage);
        }

        if (managerService.GetManagerById(managerId) == null)
        {
            return this.NotFound();
        }

        manager.Id = managerId;
        Manager? updatedManager = managerService.UpdateManager(manager);

        if (updatedManager == null)
        {
            return this.NotFound();
        }

        return this.Ok(updatedManager);
    }

    [HttpDelete("{managerId:int}")]
    public ActionResult<Manager> Delete(int managerId)
    {
        Manager? deletedManager = managerService.DeleteManager(managerId);

        if (deletedManager == null)
        {
            return this.NotFound();
        }

        return this.Ok(deletedManager);
    }
}

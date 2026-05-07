using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientRepo clientRepo;

    public ClientsController(IClientRepo clientRepo)
    {
        this.clientRepo = clientRepo;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Client>> GetAll()
    {
        return Ok(clientRepo.GetAll());
    }

    [HttpGet("{clientId:int}")]
    public ActionResult<Client> GetById(int clientId)
    {
        Client? client = clientRepo.GetById(clientId);
        return client == null ? NotFound() : Ok(client);
    }

    [HttpPost]
    public ActionResult<Client> Add(Client client)
    {
        clientRepo.Add(client);
        return CreatedAtAction(nameof(GetById), new { clientId = client.Id }, client);
    }

    [HttpPut("{clientId:int}")]
    public ActionResult<Client> Update(int clientId, Client client)
    {
        if (clientRepo.GetById(clientId) == null)
        {
            return NotFound();
        }

        client.Id = clientId;
        Client? updatedClient = clientRepo.Update(client);
        return updatedClient == null ? NotFound() : Ok(updatedClient);
    }

    [HttpDelete("{clientId:int}")]
    public ActionResult<Client> Delete(int clientId)
    {
        Client? deletedClient = clientRepo.Delete(clientId);
        return deletedClient == null ? NotFound() : Ok(deletedClient);
    }
}

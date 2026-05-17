using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController(IClientRepository clientRepository) : ControllerBase
{
    private const string MissingClientDataErrorMessage = "Client data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Client>> GetAll()
    {
        return this.Ok(clientRepository.GetAll());
    }

    [HttpGet("{clientId:int}")]
    public ActionResult<Client> GetById(int clientId)
    {
        Client? client = clientRepository.GetById(clientId);

        if (client == null)
        {
            return this.NotFound();
        }

        return this.Ok(client);
    }

    [HttpPost]
    public ActionResult<Client> Add([FromBody] Client client)
    {
        if (client == null)
        {
            return this.BadRequest(MissingClientDataErrorMessage);
        }

        clientRepository.Add(client);

        return this.CreatedAtAction(nameof(this.GetById), new { clientId = client.Id }, client);
    }
    [HttpPut("{clientId:int}")]
    public ActionResult<Client> Update(int clientId, [FromBody] Client client)
    {
        if (client == null)
        {
            return this.BadRequest(MissingClientDataErrorMessage);
        }

        if (clientRepository.GetById(clientId) == null)
        {
            return this.NotFound();
        }

        client.Id = clientId;
        Client? updatedClient = clientRepository.Update(client);

        if (updatedClient == null)
        {
            return this.NotFound();
        }

        return this.Ok(updatedClient);
    }

    [HttpDelete("{clientId:int}")]
    public ActionResult<Client> Delete(int clientId)
    {
        Client? deletedClient = clientRepository.Delete(clientId);

        if (deletedClient == null)
        {
            return this.NotFound();
        }

        return this.Ok(deletedClient);
    }
}
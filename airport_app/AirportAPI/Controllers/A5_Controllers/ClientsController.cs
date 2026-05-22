using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers.A5_Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController(IClientService clientService) : ControllerBase
{
    private const string MissingClientDataErrorMessage = "Client data cannot be null.";

    [HttpGet]
    public ActionResult<IEnumerable<Client>> GetAll()
    {
        return this.Ok(clientService.GetAllClients());
    }

    [HttpGet("{clientId:int}")]
    public ActionResult<Client> GetById(int clientId)
    {
        Client? client = clientService.GetClientById(clientId);

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

        clientService.AddClient(client);

        return this.CreatedAtAction(nameof(this.GetById), new { clientId = client.Id }, client);
    }

    [HttpPut("{clientId:int}")]
    public ActionResult<Client> Update(int clientId, [FromBody] Client client)
    {
        if (client == null)
        {
            return this.BadRequest(MissingClientDataErrorMessage);
        }

        if (clientService.GetClientById(clientId) == null)
        {
            return this.NotFound();
        }

        client.Id = clientId;
        Client? updatedClient = clientService.UpdateClient(client);

        if (updatedClient == null)
        {
            return this.NotFound();
        }

        return this.Ok(updatedClient);
    }

    [HttpDelete("{clientId:int}")]
    public ActionResult<Client> Delete(int clientId)
    {
        Client? deletedClient = clientService.DeleteClient(clientId);

        if (deletedClient == null)
        {
            return this.NotFound();
        }

        return this.Ok(deletedClient);
    }
}
using System.Net;
using System.Net.Http.Json;
using AirportApp.Data.Repositories.Proxies;

namespace AirportApp.Data.Services.Proxies;

public class ClientServiceProxy : RepositoryProxyBase, IClientService
{
    public ClientServiceProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public void AddClient(Client client)
    {
        this.Post("api/clients", client);
    }

    public Client? DeleteClient(int clientId)
    {
        ClientDto? client = this.DeleteForResult<ClientDto>($"api/clients/{clientId}");
        return client == null ? null : MapClient(client);
    }

    public IEnumerable<Client> GetAllClients()
    {
        return this.GetList<ClientDto>("api/clients")
            .Select(MapClient)
            .ToList();
    }

    public Client GetAnyClient()
    {
        IEnumerable<Client> clients = this.GetAllClients();
        Client? firstClient = null;

        foreach (Client client in clients)
        {
            firstClient = client;
            break;
        }

        if (firstClient == null)
        {
            throw new InvalidOperationException("No clients are currently registered in the system.");
        }

        return firstClient;
    }

    public Client GetClientById(int clientId)
    {
        Client? client = this.GetClientByIdOptional(clientId);
        return client ?? throw new InvalidOperationException("Client not found.");
    }

    public Client? UpdateClient(Client client)
    {
        ClientDto? updatedClient = this.PostUpdate(client);
        return updatedClient == null ? null : MapClient(updatedClient);
    }

    private Client? GetClientByIdOptional(int clientId)
    {
        ClientDto? client = this.GetOptional<ClientDto>($"api/clients/{clientId}");
        return client == null ? null : MapClient(client);
    }

    private ClientDto? PostUpdate(Client client)
    {
        using HttpResponseMessage response = this.HttpClient.PutAsJsonAsync($"api/clients/{client.Id}", client).GetAwaiter().GetResult();
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<ClientDto>(JsonOptions).GetAwaiter().GetResult();
    }

    private static Client MapClient(ClientDto client)
    {
        return new Client(client.Id, client.Name ?? string.Empty);
    }

    private sealed class ClientDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}

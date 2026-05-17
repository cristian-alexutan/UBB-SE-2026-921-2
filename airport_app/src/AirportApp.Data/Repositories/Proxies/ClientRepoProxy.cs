using System.Net;
using System.Net.Http.Json;

namespace AirportApp.Data.Repositories.Proxies;

public class ClientRepoProxy : RepositoryProxyBase, IClientRepository
{
    public ClientRepoProxy(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public IEnumerable<Client> GetAll()
    {
        return this.GetList<ClientDto>("api/clients")
            .Select(MapClient)
            .ToList();
    }

    public Client? GetById(int clientId)
    {
        ClientDto? client = this.GetOptional<ClientDto>($"api/clients/{clientId}");
        return client == null ? null : MapClient(client);
    }

    public void Add(Client client)
    {
        this.Post("api/clients", client);
    }

    public Client? Delete(int clientId)
    {
        ClientDto? client = this.DeleteForResult<ClientDto>($"api/clients/{clientId}");
        return client == null ? null : MapClient(client);
    }

    public Client? Update(Client client)
    {
        ClientDto? updatedClient = this.PostUpdate(client);
        return updatedClient == null ? null : MapClient(updatedClient);
    }

    private static Client MapClient(ClientDto client)
    {
        return new Client(client.Id, client.Name ?? string.Empty);
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

    private sealed class ClientDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}

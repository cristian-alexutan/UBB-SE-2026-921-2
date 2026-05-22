namespace AirportLib.Services.Services.Interfaces
{
    public interface IClientService
    {
        void AddClient(Client client);
        Client? DeleteClient(int clientId);
        IEnumerable<Client> GetAllClients();
        Client? GetAnyClient();
        Client GetClientById(int clientId);
        Client? UpdateClient(Client client);
    }
}
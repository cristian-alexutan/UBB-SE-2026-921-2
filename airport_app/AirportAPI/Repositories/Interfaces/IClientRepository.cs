namespace AirportAPI.Repositories.Interfaces
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAll();

        Client? GetById(int clientId);

        void Add(Client client);

        Client? Delete(int clientId);

        Client? Update(Client client);
    }
}



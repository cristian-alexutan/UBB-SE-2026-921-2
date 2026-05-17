using System.Collections.Generic;
using AirportApp.Data.Domain;

namespace AirportApp.Data.Repositories.Interfaces
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

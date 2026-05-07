using System.Collections.Generic;
using AirportAPI.Domain;

namespace AirportAPI.Repositories.Interfaces
{
    public interface IClientRepo
    {
        IEnumerable<Client> GetAll();

        Client? GetById(int clientId);

        void Add(Client client);

        Client? Delete(int clientId);

        Client? Update(Client client);
    }
}



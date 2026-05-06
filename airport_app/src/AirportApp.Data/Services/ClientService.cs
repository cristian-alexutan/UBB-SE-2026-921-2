using System;
using System.Collections.Generic;
using System.Linq;
using AirportApp.Data.Services.Interfaces;
using AirportApp.Data.Domain;
using AirportApp.Data.Repositories.Interfaces;

namespace AirportApp.Data.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepo clientRepo;

        public ClientService(IClientRepo clientRepo)
        {
            this.clientRepo = clientRepo;
        }

        public IEnumerable<Client> GetAllClients()
        {
            return clientRepo.GetAll();
        }

        public Client GetClientById(int clientId)
        {
            return clientRepo.GetById(clientId);
        }

        public void AddClient(Client client)
        {
            if (client == null)
            {
                throw new Exception("Client must not be null");
            }

            if (string.IsNullOrWhiteSpace(client.Name))
            {
                throw new Exception("Name field must not be empty");
            }

            clientRepo.Add(client);
        }

        public Client? DeleteClient(int clientId)
        {
            return clientRepo.Delete(clientId);
        }

        public Client? UpdateClient(Client client)
        {
            if (client == null)
            {
                throw new Exception("Client must not be null");
            }
            if (string.IsNullOrWhiteSpace(client.Name))
            {
                throw new Exception("Name field must not be empty");
            }
            return clientRepo.Update(client);
        }

        public Client? GetAnyClient()
        {
            Client? client = clientRepo.GetAll().FirstOrDefault();
            if (client == null)
            {
                throw new Exception();
            }
            return client;
        }
    }
}
namespace AirportApp.Data.Services
{
    public class ClientService(IClientRepository clientRepository) : IClientService
    {
        private const string NullClientErrorMessage = "The client entity cannot be null.";
        private const string EmptyNameErrorMessage = "The client name field must not be empty.";
        private const string NoClientsFoundErrorMessage = "No clients are currently registered in the system.";

        public IEnumerable<Client> GetAllClients()
        {
            return clientRepository.GetAll();
        }

        public Client? GetClientById(int clientId)
        {
            return clientRepository.GetById(clientId);
        }

        public void AddClient(Client clientToAdd)
        {
            if (clientToAdd == null)
            {
                throw new ArgumentNullException(nameof(clientToAdd), NullClientErrorMessage);
            }

            if (string.IsNullOrWhiteSpace(clientToAdd.Name))
            {
                throw new ArgumentException(EmptyNameErrorMessage, nameof(clientToAdd));
            }

            clientRepository.Add(clientToAdd);
        }

        public Client? DeleteClient(int clientId)
        {
            return clientRepository.Delete(clientId);
        }

        public Client? UpdateClient(Client clientToUpdate)
        {
            if (clientToUpdate == null)
            {
                throw new ArgumentNullException(nameof(clientToUpdate), NullClientErrorMessage);
            }

            if (string.IsNullOrWhiteSpace(clientToUpdate.Name))
            {
                throw new ArgumentException(EmptyNameErrorMessage, nameof(clientToUpdate));
            }

            return clientRepository.Update(clientToUpdate);
        }

        public Client GetAnyClient()
        {
            IEnumerable<Client> allClientsList = clientRepository.GetAll();
            Client? firstAvailableClient = null;

            foreach (Client client in allClientsList)
            {
                firstAvailableClient = client;
                break;
            }

            if (firstAvailableClient == null)
            {
                throw new InvalidOperationException(NoClientsFoundErrorMessage);
            }

            return firstAvailableClient;
        }
    }
}
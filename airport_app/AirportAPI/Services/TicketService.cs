using AirportAPI.Repositories.Interfaces;

namespace AirportAPI.Services
{
    public class TicketService(ITicketRepository ticketRepository) : ITicketService
    {
        private const string InvalidSubcategoryErrorMessage = "The provided subcategory name cannot be null or empty.";

        public int CountTicketsBySubcategory(string subcategoryName)
        {
            if (string.IsNullOrEmpty(subcategoryName))
            {
                throw new ArgumentException(InvalidSubcategoryErrorMessage, nameof(subcategoryName));
            }

            IEnumerable<Ticket> allTicketsList = ticketRepository.GetAll();
            int matchingTicketCount = 0;

            foreach (Ticket ticketInstance in allTicketsList)
            {
                if (ticketInstance.Subcategory == subcategoryName)
                {
                    matchingTicketCount++;
                }
            }

            return matchingTicketCount;
        }

        public void AddTicket(Ticket ticketToAdd)
        {
            ticketRepository.Add(ticketToAdd);
        }
    }
}
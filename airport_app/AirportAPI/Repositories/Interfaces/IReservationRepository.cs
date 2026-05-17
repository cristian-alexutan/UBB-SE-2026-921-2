namespace AirportAPI.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        IEnumerable<Reservation> GetAll();
        Reservation GetById(int reservationId);
        void Add(Reservation reservation);
        void Delete(int reservationId);
        void Update(Reservation reservation);
    }
}



using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationsController(IReservationRepository reservationRepository) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Reservation>> GetAll()
        {
            return this.Ok(reservationRepository.GetAll());
        }

        [HttpGet("{reservationId}")]
        public ActionResult<Reservation> GetById(int reservationId)
        {
            Reservation? reservation = reservationRepository.GetById(reservationId);

            if (reservation == null)
            {
                return this.NotFound();
            }

            return this.Ok(reservation);
        }

        [HttpPost]
        public ActionResult Add([FromBody] ReservationRequest request)
        {
            if (request == null)
            {
                return this.BadRequest("Reservation data is required.");
            }

            Reservation newReservation = MapReservation(request);
            reservationRepository.Add(newReservation);
            return this.CreatedAtAction(nameof(this.GetById), new { reservationId = newReservation.Id }, newReservation);
        }

        [HttpPut]
        public ActionResult Update([FromBody] ReservationRequest request)
        {
            if (request == null)
            {
                return this.BadRequest("Reservation data is required.");
            }

            Reservation reservationToUpdate = MapReservation(request);
            reservationRepository.Update(reservationToUpdate);
            return this.NoContent();
        }

        [HttpDelete("{reservationId}")]
        public ActionResult Delete(int reservationId)
        {
            reservationRepository.Delete(reservationId);
            return this.NoContent();
        }

        private static Reservation MapReservation(ReservationRequest request)
        {
            return new Reservation
            {
                Id = request.Id,
                ReservationCart = new Cart { Id = request.CartId },
                Active = request.Active,
                ReservationDate = request.ReservationDate
            };
        }
    }

    public sealed record ReservationRequest(int Id, int CartId, bool Active, DateTime ReservationDate);
}

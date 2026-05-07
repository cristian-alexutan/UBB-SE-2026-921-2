using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace AirportAPI.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController(IReservationRepo reservationRepository) : ControllerBase
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
        public ActionResult Add([FromBody] Reservation newReservation)
        {
            if (newReservation == null)
            {
                return this.BadRequest("Reservation data is required.");
            }

            reservationRepository.Add(newReservation);
            return this.Ok();
        }

        [HttpPut]
        public ActionResult Update([FromBody] Reservation reservationToUpdate)
        {
            reservationRepository.Update(reservationToUpdate);
            return this.NoContent();
        }

        [HttpDelete("{reservationId}")]
        public ActionResult Delete(int reservationId)
        {
            reservationRepository.Delete(reservationId);
            return this.NoContent();
        }
    }
}
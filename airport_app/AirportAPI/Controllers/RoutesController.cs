using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Route = AirportAPI.Domain.Route;

namespace AirportAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController(IRouteRepository routeRepository) : ControllerBase
    {
        private const string NullRouteDataErrorMessage = "Route data cannot be null.";

        [HttpGet]
        public ActionResult<IEnumerable<Route>> GetAll()
        {
            return this.Ok(routeRepository.GetAllRoutes());
        }

        [HttpGet("{routeId:int}")]
        public ActionResult<Route> GetById(int routeId)
        {
            Route? route = routeRepository.GetRouteById(routeId);

            if (route == null)
            {
                return this.NotFound();
            }

            return this.Ok(route);
        }

        [HttpPost]
        public ActionResult Add([FromBody] RouteRequest newRoute)
        {
            if (newRoute == null)
            {
                return this.BadRequest(NullRouteDataErrorMessage);
            }

            int generatedId = routeRepository.AddRoute(ToRoute(newRoute));

            return this.CreatedAtAction(nameof(this.GetById), new { routeId = generatedId }, generatedId);
        }

        [HttpPut("{routeId:int}")]
        public IActionResult Update(int routeId, [FromBody] RouteRequest routeToUpdate)
        {
            if (routeToUpdate == null)
            {
                return this.BadRequest(NullRouteDataErrorMessage);
            }

            if (routeRepository.GetRouteById(routeId) == null)
            {
                return this.NotFound();
            }

            Route route = ToRoute(routeToUpdate);
            route.Id = routeId;

            routeRepository.UpdateRoute(route);

            return this.NoContent();
        }

        [HttpDelete("{routeId:int}")]
        public IActionResult Delete(int routeId)
        {
            if (routeRepository.GetRouteById(routeId) == null)
            {
                return this.NotFound();
            }

            routeRepository.DeleteRoute(routeId);

            return this.NoContent();
        }

        private static Route ToRoute(RouteRequest request)
        {
            return new Route
            {
                Id = request.Id,
                RouteType = request.RouteType,
                RecurrenceInterval = request.RecurrenceInterval,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DepartureTime = request.DepartureTime,
                ArrivalTime = request.ArrivalTime,
                Capacity = request.Capacity,
                Company = new Company { Id = request.CompanyId },
                Airport = new Airport { Id = request.AirportId }
            };
        }
    }

    public sealed record RouteRequest(
        int Id,
        string RouteType,
        int RecurrenceInterval,
        DateOnly StartDate,
        DateOnly EndDate,
        TimeOnly DepartureTime,
        TimeOnly ArrivalTime,
        int Capacity,
        int CompanyId,
        int AirportId);
}

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
        public ActionResult Add([FromBody] Route newRoute)
        {
            if (newRoute == null)
            {
                return this.BadRequest(NullRouteDataErrorMessage);
            }

            int generatedId = routeRepository.AddRoute(newRoute);

            return this.CreatedAtAction(nameof(this.GetById), new { routeId = generatedId }, generatedId);
        }

        [HttpPut("{routeId:int}")]
        public IActionResult Update(int routeId, [FromBody] Route routeToUpdate)
        {
            if (routeToUpdate == null)
            {
                return this.BadRequest(NullRouteDataErrorMessage);
            }

            if (routeRepository.GetRouteById(routeId) == null)
            {
                return this.NotFound();
            }

            routeToUpdate.Id = routeId;

            routeRepository.UpdateRoute(routeToUpdate);

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
    }
}
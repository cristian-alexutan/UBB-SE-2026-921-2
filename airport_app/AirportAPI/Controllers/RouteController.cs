using AirportAPI.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Route = AirportAPI.Domain.Route;

namespace AirportAPI.Controllers
{
    [ApiController]
    [Route("api/route")]
    public class RouteController(IRouteRepository routeRepository) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Route>> GetAll()
        {
            return this.Ok(routeRepository.GetAllRoutes());
        }

        [HttpGet("{routeId}")]
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
        public ActionResult<int> Add([FromBody] Route newRoute)
        {
            if (newRoute == null)
            {
                return this.BadRequest("Route data is required.");
            }

            int generatedId = routeRepository.AddRoute(newRoute);
            return this.Ok(generatedId);
        }

        [HttpPut]
        public ActionResult Update([FromBody] Route routeToUpdate)
        {
            routeRepository.UpdateRoute(routeToUpdate);
            return this.NoContent();
        }

        [HttpDelete("{routeId}")]
        public ActionResult Delete(int routeId)
        {
            routeRepository.DeleteRoute(routeId);
            return this.NoContent();
        }
    }
}
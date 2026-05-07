using System.Net.Http.Json;

namespace AirportApp.WinUI.Proxies
{
    public class RouteRepositoryProxy(HttpClient httpClient) : IRouteRepository
    {
        private const string ApiBaseRoute = "api/routes";

        public List<Route> GetAllRoutes()
        {
            var response = httpClient.GetFromJsonAsync<List<Route>>(ApiBaseRoute).Result;
            return response ?? new List<Route>();
        }

        public Route? GetRouteById(int routeId)
        {
            string requestUrl = $"{ApiBaseRoute}/{routeId}";
            return httpClient.GetFromJsonAsync<Route>(requestUrl).Result;
        }

        public int AddRoute(Route newRoute)
        {
            var response = httpClient.PostAsJsonAsync(ApiBaseRoute, newRoute).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<int>().Result;
            }

            return 0;
        }

        public void UpdateRoute(Route routeToUpdate)
        {
            _ = httpClient.PutAsJsonAsync(ApiBaseRoute, routeToUpdate).Result;
        }

        public void DeleteRoute(int routeId)
        {
            string requestUrl = $"{ApiBaseRoute}/{routeId}";
            _ = httpClient.DeleteAsync(requestUrl).Result;
        }
    }
}
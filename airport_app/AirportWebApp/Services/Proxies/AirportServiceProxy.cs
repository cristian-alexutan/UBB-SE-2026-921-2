using AirportWebApp.Services.Proxies;

namespace AirportWebApp.Services.Proxies
{
    public class AirportServiceProxy : RepositoryProxyBase, IAirportService
    {
        public AirportServiceProxy(HttpClient httpClient) : base(httpClient)
        {
        }

        public List<Airport> GetAllAirports()
        {
            return GetList<Airport>("api/airport");
        }

        public Airport? GetAirportById(int airportId)
        {
            return GetOptional<Airport>($"api/airport/{airportId}");
        }

        public int AddAirport(string airportCode, string airportName, string city)
        {
            var payload = new { AirportCode = airportCode, AirportName = airportName, City = city };
            return this.PostForResult<object, int>("api/airport", payload);
        }

        public void UpdateAirport(int airportId, string? newCity = null, string? newName = null, string? newCode = null)
        {
            var payload = new { NewCity = newCity, NewName = newName, NewCode = newCode };
            this.Put($"api/airport/{airportId}", payload);
        }

        public void SaveAirport(int airportId, string airportCode, string airportName, string city)
        {
            var payload = new { AirportCode = airportCode, AirportName = airportName, City = city };
            this.Put($"api/airport/{airportId}", payload);
        }

        public void DeleteAirportUsingId(int airportId)
        {
            Delete($"api/airport/{airportId}");
        }
        public bool HasFlights(int airportId)
        {
            return GetRequired<bool>($"api/airport/{airportId}/has-flights");
        }

        public string GetDeleteWarningMessage(int airportId)
        {
            var response = this.GetRequired<DeleteWarningResponse>($"api/airport/{airportId}/delete-warning");
            return response.WarningMessage;
        }

        public record DeleteWarningResponse(string WarningMessage);
    }
}
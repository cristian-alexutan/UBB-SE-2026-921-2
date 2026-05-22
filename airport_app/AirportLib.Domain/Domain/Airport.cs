namespace AirportLib.Domain.Domain
{
    public class Airport
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public Airport()
        {
        }
    }
}

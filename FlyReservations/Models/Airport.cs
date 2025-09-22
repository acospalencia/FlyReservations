namespace FlyReservations.Models
{
    public class Airport
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string AirportName { get; set; }

        public ICollection<Flight> FlightsOrigin { get; set; }
        public ICollection<Flight> FlightsDestination { get; set; }
    }

}

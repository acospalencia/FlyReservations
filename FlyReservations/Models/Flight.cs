namespace FlyReservations.Models
{
    public class Flight
    {
        public int FlightCode { get; set; }  // PK
        public int AirportId { get; set; }
        public int OriginAirportId { get; set; }
        public int DestinationAirportId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan Duration { get; set; }

        public int IdPlane { get; set; }
        public Airplane Airplane { get; set; }

        public Airport OriginAirport { get; set; }
        public Airport DestinationAirport { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }

}

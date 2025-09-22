namespace FlyReservations.Models
{
    public class Airplane
    {
        public int Id { get; set; }
        public int Capacity { get; set; }

        public ICollection<Seat> Seats { get; set; }
        public ICollection<Flight> Flights { get; set; }
    }

}

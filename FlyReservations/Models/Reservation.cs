namespace FlyReservations.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FlightId { get; set; }  // FlightCode como FK
        public int SeatId { get; set; }
        public DateTime Date { get; set; }
        public string QrCode { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public Flight Flight { get; set; }
        public Seat Seat { get; set; }
    }

}

namespace FlyReservations.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int IdPlane { get; set; }
        public string SeatType { get; set; } 
        public string Status { get; set; }   

        public Airplane Airplane { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }

}

namespace FlyReservations.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Passport { get; set; }
        public string Password { get; set; }

        // Relaciones
        public int IdRol { get; set; }
        public Rol Rol { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }

}

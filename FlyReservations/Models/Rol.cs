namespace FlyReservations.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }

}

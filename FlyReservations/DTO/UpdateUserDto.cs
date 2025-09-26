namespace FlyReservations.DTO
{
    public record UpdateUserDto
    (
        int Id,
       string Name,
        string LastName,
        string Email,
        string Phone,
        string Passport,
        string Password

    );
}

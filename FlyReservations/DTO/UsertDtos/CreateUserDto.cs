namespace FlyReservations.DTO.UsertDtos
{
    public record CreateUserDto
    (
        string Name,
        string LastName,
        string Email,
        string Phone,
        string Passport,
        string Password,
        int IdRol 

    );
}

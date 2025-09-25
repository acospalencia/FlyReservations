namespace FlyReservations.DTO.SeatDTOs
{
    public record SeatDto
    (
    int Id,
    int IdPlane,
    string SeatType,
    string Status
    );
}

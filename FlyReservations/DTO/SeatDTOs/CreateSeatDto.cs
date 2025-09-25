namespace FlyReservations.DTO.SeatDTOs
{
    public record CreateSeatDto
    (
        int IdPlane,
        string SeatType,
        string Status
        );
}

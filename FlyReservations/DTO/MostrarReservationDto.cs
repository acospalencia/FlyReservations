namespace FlyReservations.DTO
{
    public record MostrarReservationDto
        (
        int Id,
        int UserId,
        int FlightId,
        int SeatId,
        DateTime Date,
        string QrCode,
        string Status

        );
    
}

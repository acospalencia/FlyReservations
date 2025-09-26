namespace FlyReservations.DTO.ReservationDtos
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

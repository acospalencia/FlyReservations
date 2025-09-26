namespace FlyReservations.DTO
{
    public record UpdateReservationDto
        (
        int UserId,
        int FlightId,
        int SeatId,
        DateTime Date,
        string QrCode,
        string Status

        );
    

    
}

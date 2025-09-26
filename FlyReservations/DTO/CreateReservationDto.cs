namespace FlyReservations.DTO
{
    public record CreateReservationDto
        (
        int UserId,
        int FlightId,
        int SeatId,
        DateTime Date,
        string QrCode,
        string Status



        );
    



  
}

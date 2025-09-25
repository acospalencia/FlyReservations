namespace FlyReservations.DTO
{
    public record CrearFlightDto
    (
        int AiportId,
        int OriginAiportId,
        int DestinationAiportId,
        DateTime Date,
        TimeSpan DepartureTime,
        TimeSpan Duration,
        int IdPlane


    );
}

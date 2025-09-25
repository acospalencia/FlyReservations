namespace FlyReservations.DTO
{
    public record ModificarFlightDto
    (
        DateTime Date,
        TimeSpan DepartureTime,
        TimeSpan Duration,
        int IdPlane
    );
}

namespace FlyReservations.DTO.FlightDtos
{
    public record ModificarFlightDto
    (
        DateTime Date,
        TimeSpan DepartureTime,
        TimeSpan Duration,
        int IdPlane
    );
}

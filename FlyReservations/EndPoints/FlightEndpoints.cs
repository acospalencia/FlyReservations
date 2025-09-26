using FlyReservations.Data;
using FlyReservations.DTO.FlightDtos;
using FlyReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyReservations.Endpoints
{
    public static class FlightEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/flights").WithTags("Flights");

            // Crear nuevo vuelo
            group.MapPost("/", async (FlyReservationBD db, CrearFlightDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (dto.AiportId <= 0)
                    errores["airportId"] = ["El aeropuerto no es válido."];

                if (dto.OriginAiportId <= 0)
                    errores["originAirportId"] = ["El aeropuerto de origen no es válido."];

                if (dto.DestinationAiportId <= 0)
                    errores["destinationAirportId"] = ["El aeropuerto de destino no es válido."];

                if (dto.DestinationAiportId == dto.OriginAiportId)
                    errores["destinationAirportId"] = ["El aeropuerto de destino no puede ser igual al de origen."];

                if (dto.Date < DateTime.UtcNow.Date)
                    errores["date"] = ["La fecha del vuelo no puede ser en el pasado."];

                if (dto.Duration <= TimeSpan.Zero)
                    errores["duration"] = ["La duración debe ser mayor a 0."];

                if (dto.IdPlane <= 0)
                    errores["idPlane"] = ["Debe seleccionar un avión válido."];

                if (errores.Count > 0) return Results.ValidationProblem(errores);

                var entity = new Flight
                {
                    // FlightCode lo asigna la BD (Identity)
                    AirportId = dto.AiportId,
                    OriginAirportId = dto.OriginAiportId,
                    DestinationAirportId = dto.DestinationAiportId,
                    Date = dto.Date,
                    DepartureTime = dto.DepartureTime,
                    Duration = dto.Duration,
                    IdPlane = dto.IdPlane
                };

                db.Flights.Add(entity);
                await db.SaveChangesAsync();

                return Results.Created($"/flights/{entity.FlightCode}", entity);
            });

            // Obtener lista de vuelos
            group.MapGet("/", async (FlyReservationBD db) =>
            {
                var flights = await db.Flights
                    .Include(f => f.Airplane)
                    .Include(f => f.OriginAirport)
                    .Include(f => f.DestinationAirport)
                    .OrderBy(f => f.Date)
                    .ThenBy(f => f.DepartureTime)
                    .ToListAsync();

                return Results.Ok(flights);
            });

            // Obtener un vuelo por su código
            group.MapGet("/{flightCode:int}", async (int flightCode, FlyReservationBD db) =>
            {
                var flight = await db.Flights
                    .Include(f => f.Airplane)
                    .Include(f => f.OriginAirport)
                    .Include(f => f.DestinationAirport)
                    .FirstOrDefaultAsync(f => f.FlightCode == flightCode);

                return flight is not null
                    ? Results.Ok(flight)
                    : Results.NotFound();
            });

            // Modificar un vuelo
            group.MapPut("/{flightCode:int}", async (int flightCode, ModificarFlightDto dto, FlyReservationBD db) =>
            {
                var flight = await db.Flights.FindAsync(flightCode);
                if (flight is null)
                    return Results.NotFound();

                flight.Date = dto.Date;
                flight.DepartureTime = dto.DepartureTime;
                flight.Duration = dto.Duration;
                flight.IdPlane = dto.IdPlane;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}

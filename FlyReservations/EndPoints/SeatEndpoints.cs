using FlyReservations.Data;
using FlyReservations.DTO.SeatDTOs;
using FlyReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyReservations.EndPoints
{
    public static class SeatEndpoints
    {
        public static void AddSeatEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/seats").WithTags("Seats");

            // Crear un nuevo Seat
            group.MapPost("/", async (FlyReservationBD db, CreateSeatDto dto) =>
            {
                if (string.IsNullOrWhiteSpace(dto.SeatType))
                {
                    var errores = new Dictionary<string, string[]>
                    {
                        ["SeatType"] = new[] { "El tipo de asiento es requerido." }
                    };
                    return Results.ValidationProblem(errores);
                }

                var entity = new Seat
                {
                    IdPlane = dto.IdPlane,
                    SeatType = dto.SeatType,
                    Status = dto.Status
                };

                db.Seats.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new SeatDto(entity.Id, entity.IdPlane, entity.SeatType, entity.Status);

                return Results.Created($"/api/seats/{entity.Id}", dtoSalida);
            });

            // Obtener todos los Seats
            group.MapGet("/", async (FlyReservationBD db) =>
            {
                var seats = await db.Seats
                    .OrderBy(s => s.SeatType)
                    .Select(s => new SeatDto(s.Id, s.IdPlane, s.SeatType, s.Status))
                    .ToListAsync();

                return Results.Ok(seats);
            });

            // Obtener Seat por ID
            group.MapGet("/{id:int}", async (int id, FlyReservationBD db) =>
            {
                var seat = await db.Seats
                    .Where(s => s.Id == id)
                    .Select(s => new SeatDto(s.Id, s.IdPlane, s.SeatType, s.Status))
                    .FirstOrDefaultAsync();

                return seat is not null
                    ? Results.Ok(seat)
                    : Results.NotFound(new { message = "Seat no encontrado." });
            });

            // Modificar Seat
            group.MapPut("/{id:int}", async (int id, FlyReservationBD db, ModifySeatDto dto) =>
            {
                var seat = await db.Seats.FindAsync(id);
                if (seat is null)
                    return Results.NotFound(new { message = "Seat no encontrado." });

                if (string.IsNullOrWhiteSpace(dto.SeatType))
                {
                    var errores = new Dictionary<string, string[]>
                    {
                        ["SeatType"] = new[] { "El tipo de asiento es requerido." }
                    };
                    return Results.ValidationProblem(errores);
                }

                seat.SeatType = dto.SeatType;
                seat.Status = dto.Status;

                await db.SaveChangesAsync();

                var dtoSalida = new SeatDto(seat.Id, seat.IdPlane, seat.SeatType, seat.Status);

                return Results.Ok(dtoSalida);
            });

            // Eliminar Seat
            group.MapDelete("/{id:int}", async (int id, FlyReservationBD db) =>
            {
                var seat = await db.Seats.FindAsync(id);
                if (seat is null)
                    return Results.NotFound(new { message = "Seat no encontrado." });

                db.Seats.Remove(seat);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}

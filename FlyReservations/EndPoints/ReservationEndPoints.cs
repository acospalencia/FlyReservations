using FlyReservations.Data;
using FlyReservations.DTO.ReservationDtos;
using FlyReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyReservations.EndPoints
{
    public  static class ReservationEndPoints
    {

        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/reservaciones").WithTags("Reservaciones").RequireAuthorization();


            group.MapPost("/", async (FlyReservationBD db, CreateReservationDto dto) => {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.UserId.ToString()))
                    errores["UserId"] = ["El id de usuario es requerido"];

                if (string.IsNullOrWhiteSpace(dto.FlightId.ToString()))
                    errores["FlightId"] = ["El id de vuelo es requerido"];

                if (string.IsNullOrWhiteSpace(dto.SeatId.ToString()))
                    errores["SeatId"] = ["El id del asiento es requerido"];

                if (string.IsNullOrWhiteSpace(dto.Date.ToString()))
                    errores["Date"] = ["La fecha es requerido"];

                if (string.IsNullOrWhiteSpace(dto.QrCode.ToString()))
                    errores["QrCode"] = [" es requerido"];

                if (string.IsNullOrWhiteSpace(dto.Status.ToString()))
                    errores["Status"] = ["El estado es requerido"];

                if (errores.Count > 0) return Results.ValidationProblem(errores);

                var entity = new Reservation
                {
                    UserId = dto.UserId,
                    FlightId = dto.FlightId,
                    SeatId = dto.SeatId,
                    Date = dto.Date,
                    QrCode = dto.QrCode,
                    Status = dto.Status,
                };

                db.Reservations.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new MostrarReservationDto(
                    entity.Id,
                    entity.UserId,
                    entity.FlightId,
                    entity.SeatId,
                    entity.Date,
                    entity.QrCode,
                    entity.Status);



                return Results.Created($"/reservaciones/{entity.Id}", dtoSalida);
            });

            group.MapGet("/", async (FlyReservationBD bd) =>
            {

                var consulta = await bd.Reservations.ToListAsync();

                var reservaciones = consulta.Select(l => new MostrarReservationDto(
                    l.Id,
                    l.UserId,
                    l.FlightId,
                    l.SeatId,
                    l.Date,
                    l.QrCode,
                    l.Status
                ))
                .OrderBy(l => l.UserId)
                .ToList();

                return Results.Ok(reservaciones);
            });

            group.MapGet("/{id}", async (int id, FlyReservationBD db) =>
            {

                var reservaciones = await db.Reservations
                .Where(l => l.Id == id)
                .Select(l => new MostrarReservationDto(

                    l.Id,
                    l.UserId,
                    l.FlightId,
                    l.SeatId,
                    l.Date,
                    l.QrCode,
                    l.Status

                    )).FirstOrDefaultAsync();

                return Results.Ok(reservaciones);

            });

            group.MapPut("/{id}", async (int id, UpdateReservationDto dto, FlyReservationBD db) =>
            {
                var reservaciones = await db.Reservations.FindAsync(id);
                if (reservaciones is null)
                    return Results.NotFound();


                reservaciones.UserId = dto.UserId;
                reservaciones.FlightId = dto.FlightId;
                reservaciones.SeatId = dto.SeatId;
                reservaciones.Date = dto.Date;
                reservaciones.QrCode = dto.QrCode;
                reservaciones.Status = dto.Status;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}

using FlyReservations.Data;
using FlyReservations.DTO.AvionDtos;
using FlyReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyReservations.Endpoints
{
    public static class AvionEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/aviones").WithTags("Aviones").RequireAuthorization();

            // Crear
            group.MapPost("/", async (FlyReservationBD db, CrearAvionDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (dto.capacidad <= 0)
                    errores["capacidad"] = ["La capacidad debe ser mayor que cero."];

                if (errores.Count > 0)
                    return Results.ValidationProblem(errores);

                var entity = new Airplane
                {
                    Capacity = dto.capacidad
                };

                db.Airplanes.Add(entity);
                await db.SaveChangesAsync();

                return Results.Created($"/aviones/{entity.Id}", new
                {
                    entity.Id,
                    entity.Capacity
                });
            });

            // Listar todos
            group.MapGet("/", async (FlyReservationBD db) =>
            {
                var aviones = await db.Airplanes
                    .Select(a => new
                    {
                        a.Id,
                        a.Capacity
                    })
                    .OrderBy(a => a.Id)
                    .ToListAsync();

                return Results.Ok(aviones);
            });

            // Obtener por Id
            group.MapGet("/{id}", async (int id, FlyReservationBD db) =>
            {
                var avion = await db.Airplanes
                    .Where(a => a.Id == id)
                    .Select(a => new
                    {
                        a.Id,
                        a.Capacity
                    })
                    .FirstOrDefaultAsync();

                if (avion is null)
                    return Results.NotFound();

                return Results.Ok(avion);
            });

            // Modificar
            group.MapPut("/{id}", async (int id, ModificarAvionDto dto, FlyReservationBD db) =>
            {
                var avion = await db.Airplanes.FindAsync(id);

                if (avion is null)
                    return Results.NotFound();

                if (dto.capacidad <= 0)
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        { "capacidad", new[] { "La capacidad debe ser mayor que cero." } }
                    });

                avion.Capacity = dto.capacidad;

                await db.SaveChangesAsync();

                return Results.NoContent();

            });

            // Eliminar
            group.MapDelete("/{id}", async (int id, FlyReservationBD db) =>
            {
                var avion = await db.Airplanes.FindAsync(id);

                if (avion is null)
                    return Results.NotFound();

                db.Airplanes.Remove(avion);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}

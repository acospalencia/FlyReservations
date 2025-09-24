using FlyReservations.Data;
using FlyReservations.DTO.AeropuertoDtos;
using FlyReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyReservations.Endpoints
{
    public static class AeropuertoEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/aeropuertos").WithTags("Aeropuertos");

            // Crear
            group.MapPost("/", async (FlyReservationBD db, CrearAeropuertoDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.nombreAeropuerto))
                    errores["nombreAeropuerto"] = ["El nombre del aeropuerto es requerido."];

                if (string.IsNullOrWhiteSpace(dto.pais))
                    errores["pais"] = ["El país es requerido."];

                if (errores.Count > 0)
                    return Results.ValidationProblem(errores);

                var entity = new Airport
                {
                    AirportName = dto.nombreAeropuerto,
                    Country = dto.pais
                };

                db.Airports.Add(entity);
                await db.SaveChangesAsync();

                return Results.Created($"/aeropuertos/{entity.Id}", entity);
            });

            // Listar todos
            group.MapGet("/", async (FlyReservationBD db) =>
            {
                var aeropuertos = await db.Airports
                    .Select(a => new
                    {
                        a.Id,
                        a.AirportName,
                        a.Country
                    })
                    .OrderBy(a => a.AirportName)
                    .ToListAsync();

                return Results.Ok(aeropuertos);
            });

            // Obtener por Id
            group.MapGet("/{id}", async (int id, FlyReservationBD db) =>
            {
                var aeropuerto = await db.Airports
                    .Where(a => a.Id == id)
                    .Select(a => new
                    {
                        a.Id,
                        a.AirportName,
                        a.Country
                    })
                    .FirstOrDefaultAsync();

                if (aeropuerto is null)
                    return Results.NotFound();

                return Results.Ok(aeropuerto);
            });

            // Modificar
            group.MapPut("/{id}", async (int id, ModificarAeropuertoDto dto, FlyReservationBD db) =>
            {
                var aeropuerto = await db.Airports.FindAsync(id);

                if (aeropuerto is null)
                    return Results.NotFound();

                aeropuerto.AirportName = dto.nombreAeropuerto;
                aeropuerto.Country = dto.pais;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            // Eliminar
            group.MapDelete("/{id}", async (int id, FlyReservationBD db) =>
            {
                var aeropuerto = await db.Airports.FindAsync(id);

                if (aeropuerto is null)
                    return Results.NotFound();

                db.Airports.Remove(aeropuerto);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}

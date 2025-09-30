using FlyReservations.Data;
using FlyReservations.DTO.RolDTOs;
using FlyReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyReservations.EndPoints
{
    public static class RolEndpoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/roles").WithTags("Roles").RequireAuthorization();

            // Crear un nuevo rol
            group.MapPost("/", async (FlyReservationBD db, CreateRolDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.RoleName))
                    errores["RoleName"] = new[] { "El nombre del rol es requerido." };

                if (errores.Any())
                    return Results.ValidationProblem(errores);

                var entity = new Rol
                {
                    RoleName = dto.RoleName
                };

                db.Roles.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new RolDto(
                    entity.Id,
                    entity.RoleName
                );

                return Results.Created($"/api/roles/{entity.Id}", dtoSalida);
            });

            // Obtener todos los roles
            group.MapGet("/", async (FlyReservationBD db) =>
            {
                var roles = await db.Roles
                    .OrderBy(r => r.RoleName) // ✅ Ordena antes de proyectar
                    .Select(r => new RolDto(
                        r.Id,
                        r.RoleName
                    ))
                    .ToListAsync();

                return Results.Ok(roles);
            });

            // Obtener rol por ID
            group.MapGet("/{id:int}", async (int id, FlyReservationBD db) =>
            {
                var rol = await db.Roles
                    .Where(r => r.Id == id)
                    .Select(r => new RolDto(
                        r.Id,
                        r.RoleName
                    ))
                    .FirstOrDefaultAsync();

                return rol is not null
                    ? Results.Ok(rol)
                    : Results.NotFound(new { message = "Rol no encontrado." });
            });

            // Modificar rol
            group.MapPut("/{id:int}", async (int id, FlyReservationBD db, ModifyRolDto dto) =>
            {
                var rol = await db.Roles.FindAsync(id);

                if (rol is null)
                    return Results.NotFound(new { message = "Rol no encontrado." });

                if (string.IsNullOrWhiteSpace(dto.RoleName))
                {
                    var errores = new Dictionary<string, string[]>
                    {
                        ["RoleName"] = new[] { "El nombre del rol es requerido." }
                    };
                    return Results.ValidationProblem(errores);
                }

                rol.RoleName = dto.RoleName;
                await db.SaveChangesAsync();

                var dtoSalida = new RolDto(
                    rol.Id,
                    rol.RoleName
                );

                return Results.Ok(dtoSalida);
            });

            // Eliminar rol
            group.MapDelete("/{id:int}", async (int id, FlyReservationBD db) =>
            {
                var rol = await db.Roles.FindAsync(id);

                if (rol is null)
                    return Results.NotFound(new { message = "Rol no encontrado." });

                db.Roles.Remove(rol);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}

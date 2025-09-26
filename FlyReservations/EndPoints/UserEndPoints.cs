using FlyReservations.Data;
using FlyReservations.DTO;
using FlyReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyReservations.EndPoints
{
    public static class UserEndPoints
    {
        public static void Add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/usuarios").WithTags("Usuarios");

            // 📌 POST - Crear usuario
            group.MapPost("/", async (FlyReservationBD db, CreateUserDto dto) =>
            {
                var errores = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(dto.Name))
                    errores["Name"] = ["El nombre es requerido"];

                if (string.IsNullOrWhiteSpace(dto.LastName))
                    errores["LastName"] = ["El apellido es requerido"];

                if (string.IsNullOrWhiteSpace(dto.Email))
                    errores["Email"] = ["El email es requerido"];

                if (string.IsNullOrWhiteSpace(dto.Password))
                    errores["Password"] = ["La contraseña es requerida"];

                if (errores.Count > 0) return Results.ValidationProblem(errores);

                var entity = new User
                {
                    Name = dto.Name,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Passport = dto.Passport,
                    Password = dto.Password
                };

                db.Users.Add(entity);
                await db.SaveChangesAsync();

                var dtoSalida = new MostrarUserDto(
                    entity.Id,
                    entity.Name,
                    entity.LastName,
                    entity.Email,
                    entity.Phone,
                    entity.Passport,
                    entity.Password
                );

                return Results.Created($"/usuarios/{entity.Id}", dtoSalida);
            });

            // 📌 GET - Obtener todos los usuarios
            group.MapGet("/", async (FlyReservationBD db) =>
            {
                var consulta = await db.Users.ToListAsync();

                var usuarios = consulta.Select(u => new MostrarUserDto(
                    u.Id,
                    u.Name,
                    u.LastName,
                    u.Email,
                    u.Phone,
                    u.Passport,
                    u.Password
                ))
                .OrderBy(u => u.Name)
                .ToList();

                return Results.Ok(usuarios);
            });

            // 📌 GET - Obtener usuario por ID
            group.MapGet("/{id}", async (int id, FlyReservationBD db) =>
            {
                var usuario = await db.Users
                    .Where(u => u.Id == id)
                    .Select(u => new MostrarUserDto(
                        u.Id,
                        u.Name,
                        u.LastName,
                        u.Email,
                        u.Phone,
                        u.Passport,
                        u.Password
                    ))
                    .FirstOrDefaultAsync();

                return Results.Ok(usuario);
            });

            // 📌 PUT - Actualizar usuario
            group.MapPut("/{id}", async (int id, UpdateUserDto dto, FlyReservationBD db) =>
            {
                var usuario = await db.Users.FindAsync(id);
                if (usuario is null)
                    return Results.NotFound();

                usuario.Name = dto.Name;
                usuario.LastName = dto.LastName;
                usuario.Email = dto.Email;
                usuario.Phone = dto.Phone;
                usuario.Passport = dto.Passport;
                usuario.Password = dto.Password;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}

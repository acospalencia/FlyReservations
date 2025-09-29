using FlyReservations.DTO.UsertDtos;
using FlyReservations.Services;
using Microsoft.AspNetCore.Builder;
namespace FlyReservations.EndPoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder AddAuthEndpoints(this
        IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/auth").WithTags("Auth");
            group.MapPost("/register", async (CreateUserDto dto, IAuthService
            authService) =>
            {
                try
                {
                    var user = await authService.RegisterAsync(dto);
                    return Results.Created($"/api/users/{user.Id}", new
                    {
                        user.Id,
                        user.Email
                    });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { message = ex.Message });
                }
            });
            group.MapPost("/login", async (LoginDto dto, IAuthService
            authService) =>
            {
                var res = await authService.LoginAsync(dto);
                if (res == null) return Results.Unauthorized();
                return Results.Ok(res);
            });
            return routes;
        }
    }
}

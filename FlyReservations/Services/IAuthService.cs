using System.Threading.Tasks;
using FlyReservations.DTO.UsertDtos;
using FlyReservations.Models;
namespace FlyReservations.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<User> RegisterAsync(CreateUserDto dto);
    }
}


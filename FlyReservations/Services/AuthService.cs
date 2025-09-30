using FlyReservations.Data;
using FlyReservations.DTO.UsertDtos;
using FlyReservations.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace FlyReservations.Services
{
    public class AuthService : IAuthService
    {
        private readonly FlyReservationBD _db;
        private readonly IConfiguration _cfg;
        private readonly PasswordHasher<User> _hasher;
        public AuthService(FlyReservationBD db, IConfiguration cfg)
        {
            _db = db;
            _cfg = cfg;
            _hasher = new PasswordHasher<User>();
        }
        public async Task<User> RegisterAsync(CreateUserDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new InvalidOperationException("Email ya registrado.");
            var user = new User
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Passport = dto.Passport,
                IdRol = dto.IdRol
            };
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return null;
            var result = _hasher.VerifyHashedPassword(user,
            user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed) return null;
            return GenerateJwtToken(user);
        }

        private AuthResponseDto GenerateJwtToken(User user)
        {
            var jwtCfg = _cfg.GetSection("Jwt");
            var key = jwtCfg["Key"];
            var issuer = jwtCfg["Issuer"];
            var audience = jwtCfg["Audience"];
            var expiresMinutes = int.Parse(jwtCfg["ExpiresMinutes"] ?? "60");
            var securityKey = new
            SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey,
            SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Rol?.RoleName ??
                user.IdRol.ToString())
            };

            var tokenDescriptor = new JwtSecurityToken(
                 issuer: issuer,
                 audience: audience,
                 claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: credentials
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(tokenDescriptor);

            return new AuthResponseDto(
                tokenString,
                tokenDescriptor.ValidTo,
                user.Id,
                user.Email,
                user.Rol?.RoleName ?? user.IdRol.ToString()
            );
        }
    }
}

namespace FlyReservations.DTO.UsertDtos
{
    public record AuthResponseDto
    (
         string Token ,
         DateTime ExpiresAt ,
         int UserId ,
         string Email ,
         string Role 
    );
}


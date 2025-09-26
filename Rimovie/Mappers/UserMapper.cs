using Rimovie.Entities;
using Rimovie.Models.Request;
using Rimovie.Models.Response;

namespace Rimovie.Mappers
{
    public static class UserMapper
    {
        public static User ToEntity(UserCreateDto dto)
        {
            return new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash, // Asumiendo que ya viene hasheado
                Role = dto.Role
            };
        }

        public static UserResponseDto ToResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
    }

}

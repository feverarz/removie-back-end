using Rimovie.Entities;
using Rimovie.Models.Auth;
using System.Threading.Tasks;

namespace Rimovie.Services.AuthService
{
    public interface IAuthService
    {
        // Valida credenciales y devuelve el usuario si son correctas
        Task<User?> ValidateUserAsync(string identifier, string password);

        // Registra un nuevo usuario y devuelve la entidad creada
        Task<User> RegisterAsync(RegisterDto dto);

        // Genera access y refresh token para un usuario válido
        Task<AuthResponseDto> LoginAsync(User user);

        // Refresca el access token usando el refresh token
        Task<AuthResponseDto?> RefreshTokensAsync(string refreshToken);

        // Elimina un refresh token específico (logout de una sesión)
        Task LogoutAsync(string refreshToken);

        // Elimina todos los refresh tokens del usuario (logout global)
        Task LogoutAllAsync(int userId);
    }

}

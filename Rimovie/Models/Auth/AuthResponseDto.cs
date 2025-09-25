namespace Rimovie.Models.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }

        // Este campo se usa internamente, pero no se devuelve al frontend
        public string? RefreshToken { get; set; }

        // Podés agregar más info si querés mostrarla en el frontend
        public string Username { get; set; }

    }
}

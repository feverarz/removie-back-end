using System;

namespace Rimovie.Models.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }              // Clave primaria
        public int UserId { get; set; }          // Relación con el usuario
        public string Token { get; set; }        // El refresh token en sí
        public DateTime ExpiresAt { get; set; }  // Fecha de expiración
    }
}

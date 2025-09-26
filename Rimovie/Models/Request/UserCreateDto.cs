using System.ComponentModel.DataAnnotations;

namespace Rimovie.Models.Request
{
    public class UserCreateDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        [RegularExpression("admin|user", ErrorMessage = "Role must be 'admin' or 'user'")]
        public string Role { get; set; }
    }
}

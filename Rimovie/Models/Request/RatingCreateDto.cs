using System.ComponentModel.DataAnnotations;

namespace Rimovie.Models.Request
{
    public class RatingCreateDto
    {
        [Required]
        [Range(1, 5)]
        public int Score { get; set; }
    }
}

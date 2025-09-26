using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rimovie.Models.Request
{
    public class FilmCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        [Required]
        public string PosterUrl { get; set; }
        [Required]
        public string TrailerUrl { get; set; }
        [Required]
        public List<string> Genres { get; set; }
    }
}

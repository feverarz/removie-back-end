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

    public class FilmImportDto
    {
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public int Year { get; set; }
        public string Poster_Url { get; set; }
        public GenderDto Gender { get; set; }
    }
    public class GenderDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


}

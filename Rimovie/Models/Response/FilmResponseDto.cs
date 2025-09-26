using System.Collections.Generic;

namespace Rimovie.Models.Response
{
    public class FilmResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public List<string> Genres { get; set; }
    }
}

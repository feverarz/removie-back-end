using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rimovie.Models.Request
{
    public class FilmDTO
    {
        public string Title { get; set; }
        public string Sinopsis { get; set; }
        public int Year { get; set; }
        public DirectorDTO Director { get; set; }
        public GenderDTO Genero { get; set; }
        public string Cover { get; set; }
        public bool WasWatched { get; set; }
        public int? Rating { get; set; } // Nullable int for rating
        public List<ActorDTO> Actors { get; set; }
        public string PosterUrl { get; set; }
        public int Id { get; set; }
    }

    public class DirectorDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class GenderDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ActorDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}

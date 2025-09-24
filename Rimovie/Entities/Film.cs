using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rimovie.Entities
{
    public class Film
    {
        [Key]
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Sinopsis { get; set; }
        public int Year { get; set; }
        public int DirectorId { get; set; }
        public int GenderId { get; set; }
        public string Cover { get; set; }
        public string PosterUrl { get; set; }
        public bool WasWatched { get; set; }
        public int? Rating { get; set; }
    }
}

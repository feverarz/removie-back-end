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
        [Column("filmId")] 
        public int FilmId { get; set; }

        [Required]
        [StringLength(150)]
        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("sinopsis")]
        public string Sinopsis { get; set; }

        [Required]
        [Column("year")]
        public int Year { get; set; }

        [Required]
        [ForeignKey("Director")]
        [Column("directorId")]
        public int DirectorId { get; set; }

        [Required]
        [ForeignKey("Gender")]
        [Column("genderId")]
        public int GenderId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("cover")]
        public string Cover { get; set; }

        [Required]
        [StringLength(50)]
        [Column("posterUrl")]
        public string PosterUrl { get; set; }

        [Required]
        [Column("wasWatched")]
        public bool WasWatched { get; set; }

        [Column("rating")]
        public int? Rating { get; set; }
    }
}

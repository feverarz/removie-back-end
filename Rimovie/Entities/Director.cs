using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rimovie.Entities
{
    public class Director
    {
        [Key]
        [Column("directorId")]
        public int DirectorId { get; set; }

        [Required]
        [StringLength(50)]
        [Column("name")]
        public string Name { get; set; }
    }
}

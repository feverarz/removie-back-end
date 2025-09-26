using Rimovie.Models.Response;
using System.Collections.Generic;

namespace Rimovie.Models
{
    public class WishListWithFilmsDto
    {
        public int Id { get; set; }         // wishlistid
        public int UserId { get; set; }     // userid
        public List<FilmResponseDto> Films { get; set; } = new();
    }
}

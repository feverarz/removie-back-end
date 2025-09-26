namespace Rimovie.Entities
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int FilmId { get; set; }
        public int Score { get; set; } // Ej: 1 a 5
    }

}

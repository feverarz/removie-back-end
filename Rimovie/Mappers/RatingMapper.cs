using Rimovie.Entities;
using Rimovie.Models.Request;
using Rimovie.Models.Response;

namespace Rimovie.Mappers
{
    public static class RatingMapper
    {
        public static Rating ToEntity(RatingCreateDto dto, int filmId, int userId)
        {
            return new Rating
            {
                FilmId = filmId,
                UserId = userId,
                Score = dto.Score
            };
        }

        public static RatingResponseDto ToResponseDto(int filmId, double averageScore)
        {
            return new RatingResponseDto
            {
                FilmId = filmId,
                Average = averageScore
            };
        }
    }
}
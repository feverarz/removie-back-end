using Rimovie.Entities;
using Rimovie.Models.Request;
using Rimovie.Models.Response;

namespace Rimovie.Mappers
{
    public static class FilmMapper
    {
        public static Film ToEntity(FilmCreateDto dto)
        {
            return new Film
            {
                Title = dto.Title,
                Description = dto.Description,
                ReleaseYear = dto.ReleaseYear,
                PosterUrl = dto.PosterUrl,
                TrailerUrl = dto.TrailerUrl,
                Genres = dto.Genres,
            };
        }

        public static FilmResponseDto ToResponseDto(Film film)
        {
            return new FilmResponseDto
            {
                Id = film.FilmId,
                Title = film.Title,
                Description = film.Description,
                ReleaseYear = film.ReleaseYear,
                PosterUrl = film.PosterUrl,
                TrailerUrl = film.TrailerUrl,
                Genres = film.Genres,
            };
        }
    }
}

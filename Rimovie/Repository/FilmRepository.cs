using Dapper;
using Rimovie.Entities;
using Rimovie.Repository.Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Rimovie.Repository
{
    public class FilmRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;

        public async Task<IEnumerable<Film>> GetAllAsync()
        {
            var query = @"SELECT * FROM ""Film""";
            using var connection = _context.CreateConnection();
            var films = await connection.QueryAsync<Film>(query);

            foreach (var film in films)
            {
                var genreQuery = @"
                    SELECT g.name
                    FROM filmgender fg
                    JOIN gender g ON g.genderid = fg.genderid
                    WHERE fg.filmid = @FilmId";

                var genres = await connection.QueryAsync<string>(genreQuery, new { FilmId = film.FilmId });
                film.Genres = genres.ToList();
            }

            return films;
        }

        public async Task<Film> GetByIdAsync(int id)
        {
            var query = @"SELECT * FROM ""Film"" WHERE filmid = @Id";
            using var connection = _context.CreateConnection();
            var film = await connection.QuerySingleOrDefaultAsync<Film>(query, new { Id = id });

            if (film is not null)
            {
                var genreQuery = @"
                    SELECT g.name
                    FROM filmgender fg
                    JOIN gender g ON g.genderid = fg.genderid
                    WHERE fg.filmid = @FilmId";

                var genres = await connection.QueryAsync<string>(genreQuery, new { FilmId = film.FilmId });
                film.Genres = genres.ToList();
            }

            return film;
        }

        public async Task<int> InsertAsync(Film film)
        {
            var query = @"
                INSERT INTO ""Film"" (title, description, releaseyear, posterurl, trailerurl)
                VALUES (@Title, @Description, @ReleaseYear, @PosterUrl, @TrailerUrl)
                RETURNING filmid";

            using var connection = _context.CreateConnection();
            var filmId = await connection.QuerySingleAsync<int>(query, film);

            if (film.Genres is not null)
            {
                foreach (var genreName in film.Genres)
                {
                    var genreId = await GetOrCreateGenreIdAsync(genreName, connection);
                    var linkQuery = "INSERT INTO filmgender (filmid, genderid) VALUES (@FilmId, @GenderId)";
                    await connection.ExecuteAsync(linkQuery, new { FilmId = filmId, GenderId = genreId });
                }
            }

            return filmId;
        }

        public async Task<bool> UpdateAsync(Film film)
        {
            var query = @"
                UPDATE ""Film"" SET
                    title = @Title,
                    description = @Description,
                    releaseyear = @ReleaseYear,
                    posterurl = @PosterUrl,
                    trailerurl = @TrailerUrl
                WHERE filmid = @FilmId";

            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, film);

            var deleteGenres = "DELETE FROM filmgender WHERE filmid = @FilmId";
            await connection.ExecuteAsync(deleteGenres, new { FilmId = film.FilmId });

            if (film.Genres is not null)
            {
                foreach (var genreName in film.Genres)
                {
                    var genreId = await GetOrCreateGenreIdAsync(genreName, connection);
                    var linkQuery = "INSERT INTO filmgender (filmid, genderid) VALUES (@FilmId, @GenderId)";
                    await connection.ExecuteAsync(linkQuery, new { FilmId = film.FilmId, GenderId = genreId });
                }
            }

            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = @"DELETE FROM ""Film"" WHERE filmid = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        private async Task<int> GetOrCreateGenreIdAsync(string name, System.Data.IDbConnection connection)
        {
            var selectQuery = "SELECT genderid FROM gender WHERE name = @Name";
            var genreId = await connection.QuerySingleOrDefaultAsync<int?>(selectQuery, new { Name = name });

            if (genreId.HasValue)
                return genreId.Value;

            var insertQuery = "INSERT INTO gender (name) VALUES (@Name) RETURNING genderid";
            return await connection.QuerySingleAsync<int>(insertQuery, new { Name = name });
        }
    }
}
using Dapper;
using Rimovie.Entities;
using Rimovie.Repository.Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rimovie.Repository
{
    public class FilmRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;

        public async Task<IEnumerable<Film>> GetAllAsync()
        {
            var query = "SELECT * FROM Film";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Film>(query);
        }

        public async Task<Film> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Film WHERE filmId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Film>(query, new { Id = id });
        }

        public async Task<int> InsertAsync(Film film)
        {
            var query = @"
            INSERT INTO Film (title, sinopsis, year, directorId, genderId, cover, posterUrl, wasWatched, rating)
            VALUES (@Title, @Sinopsis, @Year, @DirectorId, @GenderId, @Cover, @PosterUrl, @WasWatched, @Rating)
            RETURNING filmId";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, film);
        }

        public async Task<bool> UpdateAsync(Film film)
        {
            var query = @"
            UPDATE Film SET
                title = @Title,
                sinopsis = @Sinopsis,
                year = @Year,
                directorId = @DirectorId,
                genderId = @GenderId,
                cover = @Cover,
                posterUrl = @PosterUrl,
                wasWatched = @WasWatched,
                rating = @Rating
            WHERE filmId = @FilmId";

            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, film);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Film WHERE filmId = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

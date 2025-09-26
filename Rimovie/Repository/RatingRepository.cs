using Dapper;
using Rimovie.Repository.Dapper;
using System.Threading.Tasks;

namespace Rimovie.Repository
{
    public class RatingRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;

        public async Task<bool> InsertOrUpdateAsync(int userId, int filmId, int score)
        {
            var query = @"
            INSERT INTO rating (userid, filmid, score)
            VALUES (@UserId, @FilmId, @Score)
            ON CONFLICT (userid, filmid)
            DO UPDATE SET score = @Score";

            using var connection = _context.CreateConnection();
            var affected = await connection.ExecuteAsync(query, new { UserId = userId, FilmId = filmId, Score = score });
            return affected > 0;
        }

        public async Task<double?> GetAverageByFilmIdAsync(int filmId)
        {
            var query = @"SELECT AVG(score) FROM rating WHERE filmid = @FilmId";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<double?>(query, new { FilmId = filmId });
        }
    }
}

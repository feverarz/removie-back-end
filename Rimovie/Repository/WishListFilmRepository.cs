using Dapper;
using Rimovie.Entities;
using Rimovie.Repository.Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rimovie.Repository
{
    public class WishListFilmRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;

        public async Task<bool> AddFilmAsync(int wishListId, int filmId)
        {
            var query = @"INSERT INTO wishlistfilm (wishlistid, filmid)
                      VALUES (@WishListId, @FilmId)";
            using var connection = _context.CreateConnection();
            var affected = await connection.ExecuteAsync(query, new { WishListId = wishListId, FilmId = filmId });
            return affected > 0;
        }

        public async Task<bool> RemoveFilmAsync(int wishListId, int filmId)
        {
            var query = @"DELETE FROM wishlistfilm
                      WHERE wishlistid = @WishListId AND filmid = @FilmId";
            using var connection = _context.CreateConnection();
            var affected = await connection.ExecuteAsync(query, new { WishListId = wishListId, FilmId = filmId });
            return affected > 0;
        }

        public async Task<IEnumerable<Film>> GetFilmsByWishListAsync(int wishListId)
        {
            var query = @"
            SELECT f.*
            FROM wishlistfilm wf
            JOIN film f ON f.filmid = wf.filmid
            WHERE wf.wishlistid = @WishListId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Film>(query, new { WishListId = wishListId });
        }
    }
}
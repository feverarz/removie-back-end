using Dapper;
using Rimovie.Entities;
using Rimovie.Models;
using Rimovie.Models.Response;
using Rimovie.Repository.Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rimovie.Repository
{
    public class WishListRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;

        public async Task<int> InsertAsync(WishList wishList)
        {
            var query = @"INSERT INTO wishlist (name, userid)
                          VALUES (@Name, @UserId)
                          RETURNING wishlistid";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, wishList);
        }

        public async Task<IEnumerable<WishList>> GetAllByUserAsync(int userId)
        {
            var query = @"SELECT * FROM wishlist WHERE userid = @UserId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<WishList>(query, new { UserId = userId });
        }

        public async Task<int?> GetDefaultWishlistIdAsync(int userId)
        {
            var query = @"SELECT wishlistid FROM wishlist WHERE userid = @UserId LIMIT 1";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<int?>(query, new { UserId = userId });
        }

        public async Task<IEnumerable<WishListWithFilmsDto>> GetWithFilmsByUserAsync(int userId)
        {
            var query = @"
        SELECT 
            w.wishlistid,
            w.userid,
            f.filmid,
            f.title,
            f.description,
            f.releaseyear,
            f.posterurl,
            f.trailerurl
        FROM wishlist w
        LEFT JOIN wishlistfilm wf ON wf.wishlistid = w.wishlistid
        LEFT JOIN film f ON f.filmid = wf.filmid
        WHERE w.userid = @UserId
        ORDER BY w.wishlistid";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync(query, new { UserId = userId });

            var lookup = new Dictionary<int, WishListWithFilmsDto>();

            foreach (var row in result)
            {
                int wishListId = (int)row.wishlistid;

                if (!lookup.ContainsKey(wishListId))
                {
                    lookup[wishListId] = new WishListWithFilmsDto
                    {
                        Id = wishListId,
                        UserId = (int)row.userid,
                        Films = new List<FilmResponseDto>()
                    };
                }

                if (row.filmid != null)
                {
                    lookup[wishListId].Films.Add(new FilmResponseDto
                    {
                        Id = (int)row.filmid,
                        Title = row.title,
                        Description = row.description,
                        ReleaseYear = row.releaseyear != null ? (int)row.releaseyear : 0,
                        PosterUrl = row.posterurl,
                        TrailerUrl = row.trailerurl
                    });
                }
            }

            return lookup.Values;
        }

        public async Task<bool> DeleteAsync(int wishListId)
        {
            var query = @"DELETE FROM wishlist WHERE wishlistid = @Id";
            using var connection = _context.CreateConnection();
            var affected = await connection.ExecuteAsync(query, new { Id = wishListId });
            return affected > 0;
        }
    }
}
using Dapper;
using Rimovie.Entities;
using Rimovie.Repository.Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rimovie.Repository
{
    public class WishListRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;
        public async Task<IEnumerable<WishList>> GetAllByUserAsync(long id)
        {
            var query = "SELECT * FROM WishList WHERE userId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<WishList>(query, new { Id = id });
        }
        public async Task<WishList> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM WishList WHERE wishListId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<WishList>(query, new { Id = id });
        }
        public async Task<int> InsertAsync(WishList wishList)
        {
            var query = @"
            INSERT INTO WishList (name, description)
            VALUES (@Name, @Description)
            RETURNING wishListId";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, wishList);
        }
        public async Task<bool> UpdateAsync(WishList wishList)
        {
            var query = @"
            UPDATE WishList SET
                name = @Name,
                description = @Description
            WHERE wishListId = @WishListId";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, wishList);
            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM WishList WHERE wishListId = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

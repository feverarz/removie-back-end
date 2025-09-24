using Dapper;
using Rimovie.Entities;
using Rimovie.Repository.Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rimovie.Repository
{
    public class UserRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var query = "SELECT * FROM \"User\"";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<User>(query);
        }
        public async Task<User> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM \"User\" WHERE userId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });
        }
        public async Task<int> InsertAsync(User user)
        {
            var query = @"
                INSERT INTO ""User"" (username, email, Permission) 
                VALUES (@Username, @Email, @Permission)
                RETURNING userId";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, user);
        }
        public async Task<bool> UpdateAsync(User user)
        {
            var query = @"
            UPDATE ""User"" SET
                username = @Username,
                email = @Email,
            WHERE userId = @UserId";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, user);
            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM \"User\" WHERE userId = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

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

        public async Task<int> InsertAsync(User user)
        {
            var query = @"
                INSERT INTO ""User"" (username, email, passwordhash, role) 
                VALUES (@Username, @Email, @PasswordHash, @Role)
                RETURNING userid";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, user);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            var query = @"SELECT * FROM users WHERE email = @Email";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(query, new { Email = email });
        }
    }
}

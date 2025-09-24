using Dapper;
using Rimovie.Entities;
using Rimovie.Repository.Dapper;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Rimovie.Repository
{
    public class GenderRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;
        public async Task<IEnumerable<Gender>> GetAllAsync()
        {
            var query = "SELECT * FROM Gender";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Gender>(query);
        }
        public async Task<Gender> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Gender WHERE genderId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefault(query, new { Id = id });
        }
        public async Task<int> InsertAsync(Gender gender)
        {
            var query = @"
            INSERT INTO Gender (name)
            VALUES (@Name)
            RETURNING genderId";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, gender);
        }
        public async Task<bool> Update(Gender gender)
        {
            var query = @"
            UPDATE Gender SET
                name = @Name
            WHERE directorId = @GenderId";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, gender);
            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Gender WHERE genderId = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

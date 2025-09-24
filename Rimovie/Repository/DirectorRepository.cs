using Dapper;
using Rimovie.Entities;
using Rimovie.Repository.Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rimovie.Repository
{
    public class DirectorRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;

        public async Task<IEnumerable<Director>> GetAllAsync()
        {
            var query = "SELECT * FROM Director";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Director>(query);
        }
        public async Task<Director> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Director WHERE directorId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Director>(query, new { Id = id });
        }
        public async Task<int> InsertAsync(Director director)
        {
            var query = @"
            INSERT INTO Director (name)
            VALUES (@Name)
            RETURNING directorId";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, director);
        }
        public async Task<bool> UpdateAsync(Director director)
        {
            var query = @"
            UPDATE Director SET
                name = @Name
            WHERE directorId = @DirectorId";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, director);
            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Director WHERE directorId = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

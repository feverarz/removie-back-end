using Dapper;
using Rimovie.Entities;
using Rimovie.Repository.Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rimovie.Repository
{
    public class ActorRepository(DapperContext context)
    {
        private readonly DapperContext _context = context;

        public async Task<IEnumerable<Actor>> GetAllAsync()
        {
            var query = "SELECT * FROM Actor";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Actor>(query);
        }
        public async Task<Actor> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Actor WHERE actorId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Actor>(query, new { Id = id });
        }
        public async Task<int> InsertAsync(Actor actor)
        {
            var query = @"
            INSERT INTO Actor (name)
            VALUES (@Name)
            RETURNING actorId";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, actor);
        }
        public async Task<bool> UpdateAsync(Actor actor)
        {
            var query = @"
            UPDATE Actor SET
                name = @Name
            WHERE actorId = @ActorId";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, actor);
            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Actor WHERE actorId = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Rimovie.Entities;
using Rimovie.Helpers;
using Rimovie.Models.Auth;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Rimovie.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _db;

        public AuthService(IConfiguration config)
        {
            _config = config;
            _db = new NpgsqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<AuthResponseDto> LoginAsync(User user)
        {
            var accessToken = JwtHelper.GenerateAccessToken(user, _config);
            var refreshToken = JwtHelper.GenerateRefreshToken();

            await _db.ExecuteAsync(@"INSERT INTO RefreshTokens (userid, token, expiresat)
                                 VALUES (@uid, @token, @exp)", new
            {
                uid = user.UserId,
                token = refreshToken,
                exp = DateTime.UtcNow.AddDays(7)
            });

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = user.Username
            };
        }

        public async Task<User> ValidateUserAsync(string identifier, string password)
        {
            var user = await _db.QueryFirstOrDefaultAsync<User>(
                @"SELECT * FROM ""User"" WHERE ""email"" = @id OR ""username"" = @id", new { id = identifier });

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var userId = await _db.ExecuteScalarAsync<int>(
                @"INSERT INTO ""User"" (""username"", ""email"", ""passwordhash"", ""permission"", ""createdat"")
              VALUES (@u, @e, @p, 'user', CURRENT_TIMESTAMP) RETURNING ""userid""", new
                {
                    u = dto.Username,
                    e = dto.Email,
                    p = hash
                });

            return new User { UserId = userId, Username = dto.Username, Role = "user" };
        }

        public async Task<AuthResponseDto> RefreshTokensAsync(string refreshToken)
        {
            var token = await _db.QueryFirstOrDefaultAsync<RefreshToken>(
                @"SELECT * FROM refreshtokens WHERE token = @token AND expiresat > NOW()", new { token = refreshToken });

            if (token == null) return null;

            var user = await _db.QueryFirstOrDefaultAsync<User>(
                @"SELECT * FROM ""user"" WHERE ""userid"" = @id", new { id = token.UserId });

            if (user == null) return null;

            await _db.ExecuteAsync(@"DELETE FROM refreshtokens WHERE token = @token", new { token = refreshToken });

            return await LoginAsync(user);
        }

        public async Task LogoutAsync(string token)
        {
            await _db.ExecuteAsync(@"DELETE FROM RefreshTokens WHERE Token = @token", new { token });
        }

        public async Task LogoutAllAsync(int userId)
        {
            await _db.ExecuteAsync(@"DELETE FROM RefreshTokens WHERE UserId = @uid", new { uid = userId });
        }
    }
}

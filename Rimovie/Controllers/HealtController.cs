using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Rimovie.Excepciones;
using Rimovie.Repository.Dapper;
using System;
using System.Threading.Tasks;

namespace Rimovie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection _connection;

        public HealthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet("test-db")]
        public IActionResult TestDb([FromServices] DapperContext context)
        {
            try
            {
                using var connection = context.CreateConnection();
                // IDbConnection does not support OpenAsync, use Open() instead
                connection.Open();
                connection.Close();
                return Ok("Conexión exitosa");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpGet("ping")]
        public IActionResult Ping() => Ok("pong");
    }
}

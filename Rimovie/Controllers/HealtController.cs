using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Rimovie.Excepciones;
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
        [HttpGet("db-status")]
        public async Task<IActionResult> CheckDatabaseConnection()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string database = null;
            string dataSource = null;
            string user = null;

            try
            {
                using var connection = new NpgsqlConnection(connectionString);

                // Intentamos acceder a las propiedades sin abrir la conexión
                database = connection.Database;
                dataSource = connection.Host;
                user = connection.UserName;

                await connection.OpenAsync();
                await connection.CloseAsync();

                return Ok(new
                {
                    status = "Conexión exitosa",
                    connectionString,
                    database,
                    dataSource,
                    user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "Error de conexión",
                    error = ex.Message,
                    connectionString,
                    database,
                    dataSource,
                    user
                });
            }
        }

        [HttpGet("connection-string")]
        public IActionResult GetConnectionString()
        {
            var configValue = _configuration.GetConnectionString("DefaultConnection");
            var envValue = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

            if (string.IsNullOrWhiteSpace(configValue) && string.IsNullOrWhiteSpace(envValue))
            {
                return Ok(new
                {
                    status = "No se encontró la cadena de conexión",
                    searchedKeys = new[]
                    {
                "ConnectionStrings:DefaultConnection (appsettings)",
                "ConnectionStrings__DefaultConnection (env)"
            },
                    suggestion = "Verificá que la variable esté definida en Railway o en appsettings.json"
                });
            }

            return Ok(new
            {
                status = "Cadena de conexión encontrada",
                fromConfiguration = configValue,
                fromEnvironment = envValue
            });
        }

        [HttpGet("env-info")]
        public IActionResult GetEnvironmentInfo([FromServices] IWebHostEnvironment env)
        {
            var info = new
            {
                environmentName = env.EnvironmentName,
                isDevelopment = env.IsDevelopment(),
                isProduction = env.IsProduction(),
                machineName = Environment.MachineName,
                osVersion = Environment.OSVersion.ToString(),
                currentDirectory = Environment.CurrentDirectory,
                dotnetVersion = Environment.Version.ToString(),
                userName = Environment.UserName,
                variables = new
                {
                    dbConnection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"),
                    aspnetEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                }
            };

            return Ok(info);
        }

        [HttpGet("debug-info")]
        public async Task<IActionResult> GetDebugInfo([FromServices] IWebHostEnvironment env)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var envVar = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

            string database = null;
            string dataSource = null;
            string user = null;
            int? filmCount = null;
            string dbError = null;

            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                database = connection.Database;
                dataSource = connection.Host;
                user = connection.UserName;

                await connection.OpenAsync();

                using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM films", connection);
                var result = await cmd.ExecuteScalarAsync();
                filmCount = Convert.ToInt32(result);

                await connection.CloseAsync();
            }
            catch (Exception ex)
            {
                dbError = ex.Message;
            }

            var info = new
            {
                environment = new
                {
                    name = env.EnvironmentName,
                    isDevelopment = env.IsDevelopment(),
                    isProduction = env.IsProduction()
                },
                system = new
                {
                    machineName = Environment.MachineName,
                    osVersion = Environment.OSVersion.ToString(),
                    dotnetVersion = Environment.Version.ToString(),
                    currentDirectory = Environment.CurrentDirectory,
                    userName = Environment.UserName
                },
                connection = new
                {
                    fromConfiguration = connectionString,
                    fromEnvironment = envVar,
                    database,
                    dataSource,
                    user,
                    filmCount,
                    dbError
                }
            };

            return Ok(info);
        }
        [HttpGet("db-raw")]
        public IActionResult GetRawDbConnection()
        {
            var raw = Environment.GetEnvironmentVariable("DB_CONNECTION");

            if (string.IsNullOrWhiteSpace(raw))
            {
                return Ok(new
                {
                    status = "Variable DB_CONNECTION no encontrada o vacía",
                    suggestion = "Verificá que esté definida en Railway → Variables"
                });
            }

            return Ok(new
            {
                status = "Variable encontrada",
                connectionString = raw
            });
        }
    }
}

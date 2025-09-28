using Npgsql;
using System;
using System.Data;

namespace Rimovie.Repository.Dapper
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext()
        {
            var rawUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            if (string.IsNullOrWhiteSpace(rawUrl))
            {
                Console.WriteLine("❌ DATABASE_URL no está definida o está vacía.");
                _connectionString = ""; // Evita romper el constructor
                return;
            }

            try
            {
                var builder = new NpgsqlConnectionStringBuilder(rawUrl);
                _connectionString = builder.ConnectionString;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al construir la cadena de conexión:");
                Console.WriteLine(ex.Message);
                _connectionString = ""; // Evita romper el constructor
            }
        }

        public IDbConnection CreateConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new InvalidOperationException("Cadena de conexión no válida.");

            return new NpgsqlConnection(_connectionString);
        }
    }
}
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RedisNet8Api.Data
{
    public class ApplicationContextDapper : IApplicationContextDapper
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ApplicationContextDapper(IConfiguration configuration) 
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<T>> GetDataAsync<T>(string query, DynamicParameters? dynamicParameters)
        {

            using (var connection = CreateConnection())
            {
                var result = await connection.QueryAsync<T>(query,dynamicParameters);
                return result.ToList();
            }
        }

        public async Task<T> GetDataSingleAsync<T>(string query, DynamicParameters? dynamicParameters)
        {

            using (var connection = CreateConnection())
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>(query,dynamicParameters);
                return result;
            }
        }

        public async Task<bool> ExecuteSqlAsync<T>(string query, DynamicParameters? dynamicParameters)
        {

            using (var connection = CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, dynamicParameters) > 0;
                return result;
            }
        }

    }
}

using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Restaurant.Infrastructure.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("RestaurantDb")
            ?? throw new InvalidOperationException("Connection string 'RestaurantDb' is missing.");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}

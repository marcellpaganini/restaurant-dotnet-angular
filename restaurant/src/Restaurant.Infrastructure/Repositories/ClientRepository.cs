using Dapper;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Infrastructure.Data;

namespace Restaurant.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ClientRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        const string sql = """
            SELECT Id, Name, Email, Phone, CreatedAt
            FROM Clients
            ORDER BY Name;
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Client>(sql);
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT Id, Name, Email, Phone, CreatedAt
            FROM Clients
            WHERE Id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Client>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Client client)
    {
        const string sql = """
            INSERT INTO Clients (Name, Email, Phone, CreatedAt)
            VALUES (@Name, @Email, @Phone, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, client);
    }

    public async Task<bool> UpdateAsync(Client client)
    {
        const string sql = """
            UPDATE Clients
            SET Name = @Name, Email = @Email, Phone = @Phone
            WHERE Id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync(sql, client) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Clients WHERE Id = @Id;";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}

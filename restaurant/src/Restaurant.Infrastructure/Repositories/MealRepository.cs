using Dapper;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Infrastructure.Data;

namespace Restaurant.Infrastructure.Repositories;

public class MealRepository : IMealRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public MealRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Meal>> GetAllAsync()
    {
        const string sql = """
            SELECT Id, Name, Description, Price, IsAvailable
            FROM Meals
            ORDER BY Name;
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Meal>(sql);
    }

    public async Task<Meal?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT Id, Name, Description, Price, IsAvailable
            FROM Meals
            WHERE Id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Meal>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Meal meal)
    {
        const string sql = """
            INSERT INTO Meals (Name, Description, Price, IsAvailable)
            VALUES (@Name, @Description, @Price, @IsAvailable);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, meal);
    }

    public async Task<bool> UpdateAsync(Meal meal)
    {
        const string sql = """
            UPDATE Meals
            SET Name = @Name,
                Description = @Description,
                Price = @Price,
                IsAvailable = @IsAvailable
            WHERE Id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync(sql, meal) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Meals WHERE Id = @Id;";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}

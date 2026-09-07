using System.Data;
using Dapper;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Infrastructure.Data;

namespace Restaurant.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public OrderRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        const string sql = """
            SELECT
                o.Id, o.ClientId, o.OrderDate, o.Status, o.TotalAmount,
                c.Name AS ClientName
            FROM Orders o
            INNER JOIN Clients c ON c.Id = o.ClientId
            ORDER BY o.OrderDate DESC;
            """;

        using var connection = _connectionFactory.CreateConnection();
        var orders = (await connection.QueryAsync<Order>(sql)).ToList();
        await AttachItemsAsync(connection, orders);
        return orders;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                o.Id, o.ClientId, o.OrderDate, o.Status, o.TotalAmount,
                c.Name AS ClientName
            FROM Orders o
            INNER JOIN Clients c ON c.Id = o.ClientId
            WHERE o.Id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        var order = await connection.QuerySingleOrDefaultAsync<Order>(sql, new { Id = id });
        if (order is null)
        {
            return null;
        }

        await AttachItemsAsync(connection, [order]);
        return order;
    }

    public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
    {
        const string sql = """
            SELECT
                o.Id, o.ClientId, o.OrderDate, o.Status, o.TotalAmount,
                c.Name AS ClientName
            FROM Orders o
            INNER JOIN Clients c ON c.Id = o.ClientId
            WHERE o.ClientId = @ClientId
            ORDER BY o.OrderDate DESC;
            """;

        using var connection = _connectionFactory.CreateConnection();
        var orders = (await connection.QueryAsync<Order>(sql, new { ClientId = clientId })).ToList();
        await AttachItemsAsync(connection, orders);
        return orders;
    }

    public async Task<int> CreateAsync(Order order)
    {
        const string orderSql = """
            INSERT INTO Orders (ClientId, OrderDate, Status, TotalAmount)
            VALUES (@ClientId, @OrderDate, @Status, @TotalAmount);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        const string itemSql = """
            INSERT INTO OrderItems (OrderId, MealId, Quantity, UnitPrice)
            VALUES (@OrderId, @MealId, @Quantity, @UnitPrice);
            """;

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var orderId = await connection.ExecuteScalarAsync<int>(orderSql, order, transaction);
            foreach (var item in order.Items)
            {
                item.OrderId = orderId;
                await connection.ExecuteAsync(itemSql, item, transaction);
            }

            transaction.Commit();
            return orderId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> UpdateStatusAsync(int orderId, string status)
    {
        const string sql = """
            UPDATE Orders
            SET Status = @Status
            WHERE Id = @OrderId;
            """;

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteAsync(sql, new { OrderId = orderId, Status = status }) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string deleteItemsSql = "DELETE FROM OrderItems WHERE OrderId = @Id;";
        const string deleteOrderSql = "DELETE FROM Orders WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(deleteItemsSql, new { Id = id }, transaction);
            var deleted = await connection.ExecuteAsync(deleteOrderSql, new { Id = id }, transaction) > 0;
            transaction.Commit();
            return deleted;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private static async Task AttachItemsAsync(IDbConnection connection, IList<Order> orders)
    {
        if (orders.Count == 0)
        {
            return;
        }

        const string sql = """
            SELECT
                oi.Id, oi.OrderId, oi.MealId, oi.Quantity, oi.UnitPrice,
                m.Name AS MealName
            FROM OrderItems oi
            INNER JOIN Meals m ON m.Id = oi.MealId
            WHERE oi.OrderId IN @OrderIds;
            """;

        var orderIds = orders.Select(o => o.Id).ToArray();
        var items = await connection.QueryAsync<OrderItem>(sql, new { OrderIds = orderIds });
        var lookup = items.GroupBy(i => i.OrderId).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var order in orders)
        {
            order.Items = lookup.TryGetValue(order.Id, out var orderItems) ? orderItems : [];
        }
    }
}

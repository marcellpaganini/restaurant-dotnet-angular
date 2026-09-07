using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
    Task<int> CreateAsync(Order order);
    Task<bool> UpdateStatusAsync(int orderId, string status);
    Task<bool> DeleteAsync(int id);
}

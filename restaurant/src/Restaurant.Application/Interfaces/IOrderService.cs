using Restaurant.Application.DTOs;

namespace Restaurant.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task<IEnumerable<OrderDto>> GetByClientIdAsync(int clientId);
    Task<OrderDto> CreateAsync(CreateOrderDto dto);
    Task<OrderDto?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto);
    Task<bool> DeleteAsync(int id);
}

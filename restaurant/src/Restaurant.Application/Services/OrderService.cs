using Restaurant.Application.DTOs;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orders;
    private readonly IClientRepository _clients;
    private readonly IMealRepository _meals;

    public OrderService(
        IOrderRepository orders,
        IClientRepository clients,
        IMealRepository meals)
    {
        _orders = orders;
        _clients = clients;
        _meals = meals;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _orders.GetAllAsync();
        return orders.Select(Map);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _orders.GetByIdAsync(id);
        return order is null ? null : Map(order);
    }

    public async Task<IEnumerable<OrderDto>> GetByClientIdAsync(int clientId)
    {
        var orders = await _orders.GetByClientIdAsync(clientId);
        return orders.Select(Map);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        if (dto.Items is null || dto.Items.Count == 0)
        {
            throw new ArgumentException("Order must contain at least one item.");
        }

        var client = await _clients.GetByIdAsync(dto.ClientId)
            ?? throw new InvalidOperationException($"Client {dto.ClientId} was not found.");

        var orderItems = new List<OrderItem>();
        decimal total = 0;

        foreach (var item in dto.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Item quantity must be greater than zero.");
            }

            var meal = await _meals.GetByIdAsync(item.MealId)
                ?? throw new InvalidOperationException($"Meal {item.MealId} was not found.");

            if (!meal.IsAvailable)
            {
                throw new InvalidOperationException($"Meal '{meal.Name}' is not available.");
            }

            orderItems.Add(new OrderItem
            {
                MealId = meal.Id,
                Quantity = item.Quantity,
                UnitPrice = meal.Price,
                Meal = meal
            });

            total += meal.Price * item.Quantity;
        }

        var order = new Order
        {
            ClientId = client.Id,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            TotalAmount = total,
            Client = client,
            Items = orderItems
        };

        order.Id = await _orders.CreateAsync(order);

        var created = await _orders.GetByIdAsync(order.Id)
            ?? throw new InvalidOperationException("Order was created but could not be reloaded.");

        return Map(created);
    }

    public async Task<OrderDto?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
    {
        var existing = await _orders.GetByIdAsync(id);
        if (existing is null)
        {
            return null;
        }

        await _orders.UpdateStatusAsync(id, dto.Status);
        existing.Status = dto.Status;
        return Map(existing);
    }

    public Task<bool> DeleteAsync(int id) => _orders.DeleteAsync(id);

    private static OrderDto Map(Order order)
    {
        var items = order.Items
            .Select(i => new OrderItemDto(
                i.Id,
                i.MealId,
                i.Meal?.Name,
                i.Quantity,
                i.UnitPrice,
                i.UnitPrice * i.Quantity))
            .ToList();

        return new OrderDto(
            order.Id,
            order.ClientId,
            order.Client?.Name,
            order.OrderDate,
            order.Status,
            order.TotalAmount,
            items);
    }
}

using Restaurant.Application.DTOs;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Application.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clients;

    public ClientService(IClientRepository clients)
    {
        _clients = clients;
    }

    public async Task<IEnumerable<ClientDto>> GetAllAsync()
    {
        var items = await _clients.GetAllAsync();
        return items.Select(Map);
    }

    public async Task<ClientDto?> GetByIdAsync(int id)
    {
        var client = await _clients.GetByIdAsync(id);
        return client is null ? null : Map(client);
    }

    public async Task<ClientDto> CreateAsync(CreateClientDto dto)
    {
        var client = new Client
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            CreatedAt = DateTime.UtcNow
        };

        client.Id = await _clients.CreateAsync(client);
        return Map(client);
    }

    public async Task<ClientDto?> UpdateAsync(int id, UpdateClientDto dto)
    {
        var existing = await _clients.GetByIdAsync(id);
        if (existing is null)
        {
            return null;
        }

        existing.Name = dto.Name;
        existing.Email = dto.Email;
        existing.Phone = dto.Phone;

        await _clients.UpdateAsync(existing);
        return Map(existing);
    }

    public Task<bool> DeleteAsync(int id) => _clients.DeleteAsync(id);

    private static ClientDto Map(Client client) =>
        new(client.Id, client.Name, client.Email, client.Phone, client.CreatedAt);
}

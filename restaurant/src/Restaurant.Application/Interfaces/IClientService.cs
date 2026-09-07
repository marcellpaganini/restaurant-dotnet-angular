using Restaurant.Application.DTOs;

namespace Restaurant.Application.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientDto>> GetAllAsync();
    Task<ClientDto?> GetByIdAsync(int id);
    Task<ClientDto> CreateAsync(CreateClientDto dto);
    Task<ClientDto?> UpdateAsync(int id, UpdateClientDto dto);
    Task<bool> DeleteAsync(int id);
}

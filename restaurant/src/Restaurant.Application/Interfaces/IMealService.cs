using Restaurant.Application.DTOs;

namespace Restaurant.Application.Interfaces;

public interface IMealService
{
    Task<IEnumerable<MealDto>> GetAllAsync();
    Task<MealDto?> GetByIdAsync(int id);
    Task<MealDto> CreateAsync(CreateMealDto dto);
    Task<MealDto?> UpdateAsync(int id, UpdateMealDto dto);
    Task<bool> DeleteAsync(int id);
}

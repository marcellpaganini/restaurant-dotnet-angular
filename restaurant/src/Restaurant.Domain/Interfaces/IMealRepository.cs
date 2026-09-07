using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces;

public interface IMealRepository
{
    Task<IEnumerable<Meal>> GetAllAsync();
    Task<Meal?> GetByIdAsync(int id);
    Task<int> CreateAsync(Meal meal);
    Task<bool> UpdateAsync(Meal meal);
    Task<bool> DeleteAsync(int id);
}

using Restaurant.Application.DTOs;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Application.Services;

public class MealService : IMealService
{
    private readonly IMealRepository _meals;

    public MealService(IMealRepository meals)
    {
        _meals = meals;
    }

    public async Task<IEnumerable<MealDto>> GetAllAsync()
    {
        var items = await _meals.GetAllAsync();
        return items.Select(Map);
    }

    public async Task<MealDto?> GetByIdAsync(int id)
    {
        var meal = await _meals.GetByIdAsync(id);
        return meal is null ? null : Map(meal);
    }

    public async Task<MealDto> CreateAsync(CreateMealDto dto)
    {
        var meal = new Meal
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            IsAvailable = dto.IsAvailable
        };

        meal.Id = await _meals.CreateAsync(meal);
        return Map(meal);
    }

    public async Task<MealDto?> UpdateAsync(int id, UpdateMealDto dto)
    {
        var existing = await _meals.GetByIdAsync(id);
        if (existing is null)
        {
            return null;
        }

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Price = dto.Price;
        existing.IsAvailable = dto.IsAvailable;

        await _meals.UpdateAsync(existing);
        return Map(existing);
    }

    public Task<bool> DeleteAsync(int id) => _meals.DeleteAsync(id);

    private static MealDto Map(Meal meal) =>
        new(meal.Id, meal.Name, meal.Description, meal.Price, meal.IsAvailable);
}

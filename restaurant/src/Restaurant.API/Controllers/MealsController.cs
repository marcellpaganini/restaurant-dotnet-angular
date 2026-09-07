using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs;
using Restaurant.Application.Interfaces;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealsController : ControllerBase
{
    private readonly IMealService _mealService;
    private readonly IMapper _mapper;

    public MealsController(IMealService mealService, IMapper mapper)
    {
        _mealService = mealService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MealDto>>> GetAll()
        => Ok(await _mealService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MealDto>> GetById(int id)
    {
        var meal = await _mealService.GetByIdAsync(id);
        return meal is null ? NotFound() : Ok(meal);
    }

    [HttpPost]
    public async Task<ActionResult<MealDto>> Create([FromBody] CreateMealDto dto)
    {
        var created = await _mealService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<MealDto>(created));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMealDto dto)
    {
        var updated = await _mealService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mealService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}

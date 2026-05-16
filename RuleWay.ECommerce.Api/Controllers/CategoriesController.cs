using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RuleWay.ECommerce.Application.Abstractions;
using RuleWay.ECommerce.Application.DTOs.Categories;

namespace RuleWay.ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoriesController(
    ICategoryService categoryService,
    IValidator<CategoryRequest> categoryRequestValidator)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var categories = await categoryService.GetAllAsync(cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var category = await categoryService.GetByIdAsync(id, cancellationToken);

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CategoryRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await categoryRequestValidator.ValidateAsync(
            request,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var category = await categoryService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CategoryRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await categoryRequestValidator.ValidateAsync(
            request,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var category = await categoryService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(category);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await categoryService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
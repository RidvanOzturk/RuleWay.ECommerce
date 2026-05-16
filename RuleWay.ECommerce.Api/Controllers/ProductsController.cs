using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RuleWay.ECommerce.Application.Abstractions;
using RuleWay.ECommerce.Application.DTOs.Products;

namespace RuleWay.ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(
    IProductService productService,
    IValidator<ProductRequest> productRequestValidator,
    IValidator<ProductFilterRequest> productFilterRequestValidator)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var products = await productService.GetAllAsync(cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(id, cancellationToken);

        return Ok(product);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> Filter(
        [FromQuery] ProductFilterRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await productFilterRequestValidator.ValidateAsync(
            request,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var products = await productService.FilterAsync(request, cancellationToken);

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] ProductRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await productRequestValidator.ValidateAsync(
            request,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var product = await productService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] ProductRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await productRequestValidator.ValidateAsync(
            request,
            cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var product = await productService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await productService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
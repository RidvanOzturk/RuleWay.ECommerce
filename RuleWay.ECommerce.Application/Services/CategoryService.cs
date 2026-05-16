using Microsoft.EntityFrameworkCore;
using RuleWay.ECommerce.Application.Abstractions;
using RuleWay.ECommerce.Application.DTOs.Categories;
using RuleWay.ECommerce.Application.Mappings;
using RuleWay.ECommerce.Domain.Exceptions;

namespace RuleWay.ECommerce.Application.Services;

public sealed class CategoryService(IApplicationDbContext applicationDbContext) : ICategoryService
{
    public async Task<IReadOnlyList<CategoryResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var categories = await applicationDbContext.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);

        return categories.Select(category => category.ToResponse()).ToList();
    }

    public async Task<CategoryResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category = await applicationDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }

        return category.ToResponse();
    }

    public async Task<CategoryResponse> CreateAsync(
        CategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var categoryName = request.Name.Trim();

        var categoryExists = await applicationDbContext.Categories
            .AsNoTracking()
            .AnyAsync(category => category.Name == categoryName, cancellationToken);

        if (categoryExists)
        {
            throw new BusinessRuleException("Category already exists.");
        }

        var category = request.ToEntity();

        await applicationDbContext.Categories.AddAsync(category, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return category.ToResponse();
    }

    public async Task<CategoryResponse> UpdateAsync(
        int id,
        CategoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var category = await applicationDbContext.Categories
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }

        var categoryName = request.Name.Trim();

        var categoryExists = await applicationDbContext.Categories
            .AsNoTracking()
            .AnyAsync(category =>
                    category.Id != id &&
                    category.Name == categoryName,
                cancellationToken);

        if (categoryExists)
        {
            throw new BusinessRuleException("Category already exists.");
        }

        category.Name = categoryName;
        category.MinimumStockQuantity = request.MinimumStockQuantity;
        category.UpdatedAt = DateTime.UtcNow;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return category.ToResponse();
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category = await applicationDbContext.Categories
            .FirstOrDefaultAsync(category => category.Id == id, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }

        var hasProducts = await applicationDbContext.Products
            .AsNoTracking()
            .AnyAsync(product => product.CategoryId == id, cancellationToken);

        if (hasProducts)
        {
            throw new BusinessRuleException(
                "Category cannot be deleted because it has related products.");
        }

        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
using Microsoft.EntityFrameworkCore;
using RuleWay.ECommerce.Application.Abstractions;
using RuleWay.ECommerce.Application.DTOs.Products;
using RuleWay.ECommerce.Application.Mappings;
using RuleWay.ECommerce.Domain.Exceptions;

namespace RuleWay.ECommerce.Application.Services;

public sealed class ProductService(IApplicationDbContext applicationDbContext) : IProductService
{
    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await applicationDbContext.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .OrderByDescending(product => product.CreatedAt)
            .ToListAsync(cancellationToken);

        return products.Select(product => product.ToResponse()).ToList();
    }

    public async Task<ProductResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var product = await applicationDbContext.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException("Product not found.");
        }

        return product.ToResponse();
    }

    public async Task<IReadOnlyList<ProductResponse>> FilterAsync(
        ProductFilterRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = applicationDbContext.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(product =>
                product.Title.Contains(search) ||
                (product.Description != null && product.Description.Contains(search)) ||
                (product.Category != null && product.Category.Name.Contains(search)));
        }

        if (request.MinStockQuantity.HasValue)
        {
            query = query.Where(product =>
                product.StockQuantity >= request.MinStockQuantity.Value);
        }

        if (request.MaxStockQuantity.HasValue)
        {
            query = query.Where(product =>
                product.StockQuantity <= request.MaxStockQuantity.Value);
        }

        var products = await query
            .OrderByDescending(product => product.CreatedAt)
            .ToListAsync(cancellationToken);

        return products.Select(product => product.ToResponse()).ToList();
    }

    public async Task<ProductResponse> CreateAsync(
        ProductRequest request,
        CancellationToken cancellationToken = default)
    {
        await ValidateProductCanBeLiveAsync(
            request.CategoryId,
            request.StockQuantity,
            request.IsLive,
            cancellationToken);

        var product = request.ToEntity();

        await applicationDbContext.Products.AddAsync(product, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(product.Id, cancellationToken);
    }

    public async Task<ProductResponse> UpdateAsync(
        int id,
        ProductRequest request,
        CancellationToken cancellationToken = default)
    {
        var product = await applicationDbContext.Products
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException("Product not found.");
        }

        await ValidateProductCanBeLiveAsync(
            request.CategoryId,
            request.StockQuantity,
            request.IsLive,
            cancellationToken);

        product.Title = request.Title.Trim();
        product.Description = request.Description?.Trim();
        product.CategoryId = request.CategoryId;
        product.StockQuantity = request.StockQuantity;
        product.IsLive = request.IsLive;
        product.UpdatedAt = DateTime.UtcNow;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(product.Id, cancellationToken);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var product = await applicationDbContext.Products
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException("Product not found.");
        }

        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateProductCanBeLiveAsync(
        int? categoryId,
        int stockQuantity,
        bool isLive,
        CancellationToken cancellationToken)
    {
        if (!isLive)
        {
            return;
        }

        if (categoryId is null)
        {
            throw new BusinessRuleException("Product must have a category to be live.");
        }

        var category = await applicationDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == categoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }

        if (stockQuantity < category.MinimumStockQuantity)
        {
            throw new BusinessRuleException(
                "Product cannot be live because its stock quantity is below the category minimum stock quantity.");
        }
    }
}
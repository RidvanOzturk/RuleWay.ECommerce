using Microsoft.EntityFrameworkCore;
using RuleWay.ECommerce.Application.Abstractions;
using RuleWay.ECommerce.Application.DTOs.Products;
using RuleWay.ECommerce.Application.Mappings;
using RuleWay.ECommerce.Domain.Exceptions;

namespace RuleWay.ECommerce.Application.Services;

public sealed class ProductService(IApplicationDbContext applicationDbContext) : IProductService
{
    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken cancellationToken = default)
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
            .Include(currentProduct => currentProduct.Category)
            .FirstOrDefaultAsync(currentProduct => currentProduct.Id == id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException("Product not found.");
        }

        return product.ToResponse();
    }

    public async Task<IReadOnlyList<ProductResponse>> FilterAsync(ProductFilterRequest request, CancellationToken cancellationToken = default)
    {
        var productQuery = applicationDbContext.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            productQuery = productQuery.Where(product =>
                product.Title.Contains(search) ||
                (product.Description != null && product.Description.Contains(search)) ||
                (product.Category != null && product.Category.Name.Contains(search)));
        }

        if (request.MinStock.HasValue)
        {
            productQuery = productQuery.Where(product =>
                product.StockQuantity >= request.MinStock.Value);
        }

        if (request.MaxStock.HasValue)
        {
            productQuery = productQuery.Where(product =>
                product.StockQuantity <= request.MaxStock.Value);
        }

        var products = await productQuery
            .OrderByDescending(product => product.CreatedAt)
            .ToListAsync(cancellationToken);

        return products.Select(product => product.ToResponse()).ToList();
    }

    public async Task<ProductResponse> CreateAsync(ProductRequest request, CancellationToken cancellationToken = default)
    {
        var categoryMinimumStockQuantity = await GetCategoryMinimumStockQuantityAsync(
            request.CategoryId,
            cancellationToken);

        var product = request.ToEntity();
        product.CategoryMinimumStockQuantity = categoryMinimumStockQuantity;

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
            .FirstOrDefaultAsync(currentProduct => currentProduct.Id == id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException("Product not found.");
        }

        var categoryMinimumStockQuantity = await GetCategoryMinimumStockQuantityAsync(
            request.CategoryId,
            cancellationToken);

        product.Title = request.Title.Trim();
        product.Description = request.Description?.Trim();
        product.CategoryId = request.CategoryId;
        product.StockQuantity = request.StockQuantity;
        product.CategoryMinimumStockQuantity = categoryMinimumStockQuantity;

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(product.Id, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await applicationDbContext.Products
            .FirstOrDefaultAsync(currentProduct => currentProduct.Id == id, cancellationToken);

        if (product is null)
        {
            return;
        }

        applicationDbContext.Products.Remove(product);

        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<int?> GetCategoryMinimumStockQuantityAsync(int? categoryId, CancellationToken cancellationToken)
    {
        if (categoryId is null)
        {
            return null;
        }

        var category = await applicationDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(currentCategory => currentCategory.Id == categoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException("Category not found.");
        }

        return category.MinimumStockQuantity;
    }
}
using RuleWay.ECommerce.Application.DTOs.Products;
using RuleWay.ECommerce.Domain.Entities;

namespace RuleWay.ECommerce.Application.Mappings;

public static class ProductMapping
{
    extension(Product product)
    {
        public ProductResponse ToResponse()
        {
            return new ProductResponse(
                product.Id,
                product.Title,
                product.Description,
                product.StockQuantity,
                product.IsLive,
                product.CategoryId,
                product.Category?.Name,
                product.Category?.MinimumStockQuantity,
                product.CreatedAt,
                product.UpdatedAt);
        }
    }

    extension(ProductRequest request)
    {
        public Product ToEntity()
        {
            return new Product
            {
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                CategoryId = request.CategoryId,
                StockQuantity = request.StockQuantity,
                IsLive = request.IsLive,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
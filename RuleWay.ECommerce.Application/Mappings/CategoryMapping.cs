using RuleWay.ECommerce.Application.DTOs.Categories;
using RuleWay.ECommerce.Domain.Entities;

namespace RuleWay.ECommerce.Application.Mappings;

public static class CategoryMapping
{
    extension(Category category)
    {
        public CategoryResponse ToResponse()
        {
            return new CategoryResponse(
                category.Id,
                category.Name,
                category.MinimumStockQuantity,
                category.CreatedAt,
                category.UpdatedAt);
        }
    }

    extension(CategoryRequest request)
    {
        public Category ToEntity()
        {
            return new Category
            {
                Name = request.Name.Trim(),
                MinimumStockQuantity = request.MinimumStockQuantity
            };
        }
    }
}
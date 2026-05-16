namespace RuleWay.ECommerce.Application.DTOs.Categories;

public sealed record CategoryResponse(
    int Id,
    string Name,
    int MinimumStockQuantity,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
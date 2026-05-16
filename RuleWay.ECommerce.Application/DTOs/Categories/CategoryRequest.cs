namespace RuleWay.ECommerce.Application.DTOs.Categories;

public sealed record CategoryRequest(
    string Name,
    int MinimumStockQuantity);
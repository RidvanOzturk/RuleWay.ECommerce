namespace RuleWay.ECommerce.Application.DTOs.Products;

public sealed record ProductRequest(
    string Title,
    string? Description,
    int? CategoryId,
    int StockQuantity);
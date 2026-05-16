namespace RuleWay.ECommerce.Application.DTOs.Products;

public sealed record ProductResponse(
    int Id,
    string Title,
    string? Description,
    int StockQuantity,
    bool IsLive,
    int? CategoryId,
    string? CategoryName,
    int? CategoryMinimumStockQuantity,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
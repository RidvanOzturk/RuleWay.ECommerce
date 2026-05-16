namespace RuleWay.ECommerce.Application.DTOs.Products;

public sealed record ProductFilterRequest(
    string? Search,
    int? MinStockQuantity,
    int? MaxStockQuantity);
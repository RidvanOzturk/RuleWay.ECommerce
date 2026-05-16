namespace RuleWay.ECommerce.Application.DTOs.Products;

public sealed record ProductFilterRequest(
    string? Search,
    int? MinStock,
    int? MaxStock);
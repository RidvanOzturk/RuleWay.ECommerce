using RuleWay.ECommerce.Domain.Common;

namespace RuleWay.ECommerce.Domain.Entities;

public sealed class Product : AuditEntity
{
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public Category? Category { get; set; }

    public int StockQuantity { get; set; }

    public int? CategoryMinimumStockQuantity { get; set; }

    public bool IsLive { get; private set; }
}
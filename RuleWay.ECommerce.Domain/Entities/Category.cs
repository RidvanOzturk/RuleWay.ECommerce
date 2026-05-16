using RuleWay.ECommerce.Domain.Common;

namespace RuleWay.ECommerce.Domain.Entities;

public sealed class Category : AuditEntity
{
    public string Name { get; set; } = default!;

    public int MinimumStockQuantity { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}
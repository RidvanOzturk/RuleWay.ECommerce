using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RuleWay.ECommerce.Domain.Entities;

namespace RuleWay.ECommerce.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);

        builder.Property(product => product.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(product => product.Description)
            .HasMaxLength(1000);

        builder.Property(product => product.StockQuantity)
            .IsRequired();

        builder.Property(product => product.CategoryMinimumStockQuantity);

        builder.Property(product => product.IsLive)
            .HasComputedColumnSql(
                "CASE WHEN [CategoryId] IS NOT NULL AND [CategoryMinimumStockQuantity] IS NOT NULL AND [StockQuantity] >= [CategoryMinimumStockQuantity] THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END",
                stored: false);

        builder.Property(product => product.CreatedAt)
            .IsRequired();

        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(product => !product.IsDeleted);
    }
}
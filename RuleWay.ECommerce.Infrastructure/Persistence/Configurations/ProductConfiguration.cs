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

        builder.Property(product => product.IsLive)
            .IsRequired();

        builder.Property(product => product.CreatedAt)
            .IsRequired();

        builder.Property(product => product.CreatedBy)
            .HasMaxLength(100);

        builder.Property(product => product.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(product => product.DeletedBy)
            .HasMaxLength(100);

        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(product => !product.IsDeleted);
    }
}
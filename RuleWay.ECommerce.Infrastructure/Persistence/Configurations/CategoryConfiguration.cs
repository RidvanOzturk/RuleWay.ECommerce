using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RuleWay.ECommerce.Domain.Entities;

namespace RuleWay.ECommerce.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(category => category.MinimumStockQuantity)
            .IsRequired();

        builder.Property(category => category.CreatedAt)
            .IsRequired();

        builder.Property(category => category.CreatedBy)
            .HasMaxLength(100);

        builder.Property(category => category.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(category => category.DeletedBy)
            .HasMaxLength(100);

        builder.HasQueryFilter(category => !category.IsDeleted);
    }
}
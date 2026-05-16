using Microsoft.EntityFrameworkCore;
using RuleWay.ECommerce.Domain.Entities;

namespace RuleWay.ECommerce.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static void ApplySeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Electronics",
                MinimumStockQuantity = 10,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "Seed"
            },
            new Category
            {
                Id = 2,
                Name = "Books",
                MinimumStockQuantity = 5,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "Seed"
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Title = "iPhone 15",
                Description = "Apple smartphone",
                CategoryId = 1,
                StockQuantity = 20,
                IsLive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "Seed"
            },
            new Product
            {
                Id = 2,
                Title = "Clean Code",
                Description = "Software development book",
                CategoryId = 2,
                StockQuantity = 8,
                IsLive = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "Seed"
            }
        );
    }
}
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
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category
            {
                Id = 2,
                Name = "Books",
                MinimumStockQuantity = 5,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Title = "iPhone 15",
                Description = "Apple smartphone",
                CategoryId = 1,
                CategoryMinimumStockQuantity = 10,
                StockQuantity = 20,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Product
            {
                Id = 2,
                Title = "Clean Code",
                Description = "Software development book",
                CategoryId = 2,
                CategoryMinimumStockQuantity = 5,
                StockQuantity = 8,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
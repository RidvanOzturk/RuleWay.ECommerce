using Microsoft.EntityFrameworkCore;
using RuleWay.ECommerce.Application.Abstractions;
using RuleWay.ECommerce.Domain.Entities;
using RuleWay.ECommerce.Infrastructure.Persistence.Seed;

namespace RuleWay.ECommerce.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Product> Products { get; set; } = default!;

    public DbSet<Category> Categories { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.ApplySeedData();

        base.OnModelCreating(modelBuilder);
    }
}
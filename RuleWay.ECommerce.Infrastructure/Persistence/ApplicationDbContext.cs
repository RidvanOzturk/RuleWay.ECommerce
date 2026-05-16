using Microsoft.EntityFrameworkCore;
using RuleWay.ECommerce.Application.Abstractions;
using RuleWay.ECommerce.Domain.Common;
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

    public override int SaveChanges()
    {
        ModifyAuditProperties();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ModifyAuditProperties();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ModifyAuditProperties()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entityEntry in ChangeTracker.Entries<AuditEntity>())
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    entityEntry.Entity.CreatedAt = utcNow;
                    break;

                case EntityState.Modified:
                    entityEntry.Entity.UpdatedAt = utcNow;
                    break;

                case EntityState.Deleted:
                    entityEntry.State = EntityState.Modified;
                    entityEntry.Entity.IsDeleted = true;
                    entityEntry.Entity.DeletedAt = utcNow;
                    break;
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using RuleWay.ECommerce.Domain.Entities;

namespace RuleWay.ECommerce.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }

    DbSet<Category> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
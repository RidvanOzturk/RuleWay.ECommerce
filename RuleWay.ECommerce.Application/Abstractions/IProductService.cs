using RuleWay.ECommerce.Application.DTOs.Products;

namespace RuleWay.ECommerce.Application.Abstractions;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ProductResponse> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductResponse>> FilterAsync(
        ProductFilterRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductResponse> CreateAsync(
        ProductRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductResponse> UpdateAsync(
        int id,
        ProductRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
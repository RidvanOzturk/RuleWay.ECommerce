using FluentValidation;
using RuleWay.ECommerce.Application.DTOs.Products;

namespace RuleWay.ECommerce.Application.Validators.Products;

public sealed class ProductFilterRequestValidator : AbstractValidator<ProductFilterRequest>
{
    public ProductFilterRequestValidator()
    {
        RuleFor(filter => filter.MinStockQuantity)
            .GreaterThanOrEqualTo(0)
            .When(filter => filter.MinStockQuantity.HasValue);

        RuleFor(filter => filter.MaxStockQuantity)
            .GreaterThanOrEqualTo(0)
            .When(filter => filter.MaxStockQuantity.HasValue);

        RuleFor(filter => filter)
            .Must(filter =>
                !filter.MinStockQuantity.HasValue ||
                !filter.MaxStockQuantity.HasValue ||
                filter.MinStockQuantity.Value <= filter.MaxStockQuantity.Value)
            .WithMessage("Minimum stock quantity cannot be greater than maximum stock quantity.");
    }
}
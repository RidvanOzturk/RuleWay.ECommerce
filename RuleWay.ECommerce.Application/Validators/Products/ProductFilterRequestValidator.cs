using FluentValidation;
using RuleWay.ECommerce.Application.DTOs.Products;

namespace RuleWay.ECommerce.Application.Validators.Products;

public sealed class ProductFilterRequestValidator : AbstractValidator<ProductFilterRequest>
{
    public ProductFilterRequestValidator()
    {
        RuleFor(filter => filter.MinStock)
            .GreaterThanOrEqualTo(0)
            .When(filter => filter.MinStock.HasValue);

        RuleFor(filter => filter.MaxStock)
            .GreaterThanOrEqualTo(0)
            .When(filter => filter.MaxStock.HasValue);

        RuleFor(filter => filter)
            .Must(filter =>
                !filter.MinStock.HasValue ||
                !filter.MaxStock.HasValue ||
                filter.MinStock.Value <= filter.MaxStock.Value)
            .WithMessage("Minimum stock cannot be greater than maximum stock.");
    }
}
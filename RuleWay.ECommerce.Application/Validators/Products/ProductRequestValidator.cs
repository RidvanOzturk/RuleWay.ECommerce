using FluentValidation;
using RuleWay.ECommerce.Application.DTOs.Products;

namespace RuleWay.ECommerce.Application.Validators.Products;

public sealed class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(product => product.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(product => product.Description)
            .MaximumLength(1000);

        RuleFor(product => product.StockQuantity)
            .GreaterThanOrEqualTo(0);

        RuleFor(product => product.CategoryId)
            .GreaterThan(0)
            .When(product => product.CategoryId.HasValue);
    }
}
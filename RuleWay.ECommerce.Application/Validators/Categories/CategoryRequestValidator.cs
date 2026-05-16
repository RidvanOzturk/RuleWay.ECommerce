using FluentValidation;
using RuleWay.ECommerce.Application.DTOs.Categories;

namespace RuleWay.ECommerce.Application.Validators.Categories;

public sealed class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(category => category.MinimumStockQuantity)
            .GreaterThanOrEqualTo(0);
    }
}
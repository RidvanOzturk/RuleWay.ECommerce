using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RuleWay.ECommerce.Api.Filters;

public sealed class FluentValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var actionArgument in context.ActionArguments.Values)
        {
            if (actionArgument is null)
            {
                continue;
            }

            var validatorType = typeof(IValidator<>).MakeGenericType(actionArgument.GetType());

            var validator = serviceProvider.GetService(validatorType);

            if (validator is not IValidator fluentValidator)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(actionArgument);

            var validationResult = await fluentValidator.ValidateAsync(
                validationContext,
                context.HttpContext.RequestAborted);

            if (!validationResult.IsValid)
            {
                context.Result = new BadRequestObjectResult(validationResult.Errors);

                return;
            }
        }

        await next();
    }
}
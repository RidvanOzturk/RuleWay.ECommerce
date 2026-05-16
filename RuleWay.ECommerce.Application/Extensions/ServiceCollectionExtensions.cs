using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RuleWay.ECommerce.Application.Abstractions;
using RuleWay.ECommerce.Application.Services;
using RuleWay.ECommerce.Application.Validators.Products;

namespace RuleWay.ECommerce.Application.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddValidatorsFromAssembly(typeof(ProductRequestValidator).Assembly);

            return services;
        }
    }
}
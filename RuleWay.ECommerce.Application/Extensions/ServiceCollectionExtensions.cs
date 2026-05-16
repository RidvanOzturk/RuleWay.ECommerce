using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RuleWay.ECommerce.Application.Abstractions;
using RuleWay.ECommerce.Application.Services;

namespace RuleWay.ECommerce.Application.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}
using FluentValidation;
using ProductApp.Application.Validators;
using ProductApp.Model.Requests;

namespace ProductApp.Application.Configuration;

public static class ValidationServices
{
    public static void SetupValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<ProductRequest>, ProductRequestValidator>();
    }
}
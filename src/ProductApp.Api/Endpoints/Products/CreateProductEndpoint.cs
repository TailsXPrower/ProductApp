using FastEndpoints;
using FluentValidation;
using ProductApp.Api.Extensions;
using ProductApp.Application.Contracts;
using ProductApp.Data.Entities;
using ProductApp.Model.DTO;
using ProductApp.Model.Requests;

namespace ProductApp.Api.Endpoints.Products;

public sealed class CreateProductEndpoint(
    IProductRepository repository,
    IValidator<ProductRequest> validator) : Endpoint<ProductRequest, ProductDto>
{
    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Create a new product.";
            s.Response<ProductDto>(201, "Product created successfully.");
            s.Response(400, "Validation error.");
        });
    }

    public override async Task HandleAsync(ProductRequest req, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(req, cancellationToken);
        if (!validation.IsValid)
        {
            validation.Errors.ForEach(AddError);
            await Send.ErrorsAsync(cancellation: cancellationToken);
            return;
        }

        var product = req.ToEntity();
        await repository.AddAsync(product, cancellationToken);

        await Send.CreatedAtAsync<GetProductByIdEndpoint>(
            new { id = product.Id },
            product.ToDto(),
            cancellation: cancellationToken);
    }
}

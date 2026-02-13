using FastEndpoints;
using FluentValidation;
using ProductApp.Application.Contracts;
using ProductApp.Data.Entities;
using ProductApp.Model.DTO;
using ProductApp.Model.Requests;

namespace ProductApp.Api.Endpoints.Products;

public sealed class CreateProductEndpoint(
    IProductRepository repository,
    IValidator<CreateProductRequest> validator) : Endpoint<CreateProductRequest, ProductDto>
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

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(req, cancellationToken);
        if (!validation.IsValid)
        {
            validation.Errors.ForEach(AddError);
            await Send.ErrorsAsync(cancellation: cancellationToken);
            return;
        }

        var product = new Product(req.Name, req.Price, req.Description);
        await repository.AddAsync(product, cancellationToken);

        await Send.CreatedAtAsync<GetProductByIdEndpoint>(
            new { id = product.Id },
            new ProductDto(product.Id, product.Name, product.Price, product.Description),
            cancellation: cancellationToken);
    }
}

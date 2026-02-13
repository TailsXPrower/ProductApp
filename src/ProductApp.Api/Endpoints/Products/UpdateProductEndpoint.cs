using FastEndpoints;
using FluentValidation;
using ProductApp.Api.Extensions;
using ProductApp.Application.Contracts;
using ProductApp.Model.DTO;
using ProductApp.Model.Requests;

namespace ProductApp.Api.Endpoints.Products;

public sealed class UpdateProductEndpoint(
    IProductRepository repository,
    IValidator<ProductRequest> validator) : Endpoint<ProductRequest, ProductDto>
{
    public override void Configure()
    {
        Put("/products/{id:int}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Update an existing product.";
            s.Params["id"] = "Product identifier";
            s.Response<ProductDto>(200, "Product updated successfully.");
            s.Response(400, "Validation error.");
            s.Response(404, "Product not found.");
        });
    }

    public override async Task HandleAsync(ProductRequest req, CancellationToken cancellationToken)
    {
        var id = Route<int>("id");

        var validation = await validator.ValidateAsync(req, cancellationToken);
        if (!validation.IsValid)
        {
            validation.Errors.ForEach(AddError);
            await Send.ErrorsAsync(cancellation: cancellationToken);
            return;
        }

        var product = await repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            await Send.NotFoundAsync(cancellationToken);
            return;
        }

        product.UpdateFromRequest(req);
        await repository.UpdateAsync(product, cancellationToken);

        await Send.OkAsync(product.ToDto(), cancellationToken);
    }
}

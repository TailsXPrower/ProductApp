using FastEndpoints;
using FluentValidation;
using ProductApp.Application.Contracts;
using ProductApp.Model.DTO;
using ProductApp.Model.Requests;

namespace ProductApp.Api.Endpoints.Products;

public sealed class UpdateProductEndpoint(
    IProductRepository repository,
    IValidator<UpdateProductRequest> validator) : Endpoint<UpdateProductRequest, ProductDto>
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

    public override async Task HandleAsync(UpdateProductRequest req, CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(req, cancellationToken);
        if (!validation.IsValid)
        {
            validation.Errors.ForEach(AddError);
            await Send.ErrorsAsync(cancellation: cancellationToken);
            return;
        }

        var product = await repository.GetByIdAsync(req.Id, cancellationToken);
        if (product is null)
        {
            await Send.NotFoundAsync(cancellationToken);
            return;
        }

        product.UpdateDetails(req.Name, req.Price, req.Description);
        await repository.UpdateAsync(product, cancellationToken);

        await Send.OkAsync(new ProductDto(product.Id, product.Name, product.Price, product.Description), cancellationToken);
    }
}

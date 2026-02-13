using FastEndpoints;
using ProductApp.Api.Extensions;
using ProductApp.Application.Contracts;
using ProductApp.Model.DTO;

namespace ProductApp.Api.Endpoints.Products;

public sealed class GetProductByIdEndpoint(IProductRepository repository) : EndpointWithoutRequest<ProductDto>
{
    public override void Configure()
    {
        Get("/products/{id:int}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Retrieve a product by identifier.";
            s.Params["id"] = "Product identifier";
            s.Response<ProductDto>(200, "Product found.");
            s.Response(404, "Product not found.");
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(Route<int>("id"), cancellationToken);
        if (product is null)
        {
            await Send.NotFoundAsync(cancellationToken);
            return;
        }

        await Send.OkAsync(product.ToDto(), cancellationToken);
    }
}

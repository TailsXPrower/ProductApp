using FastEndpoints;
using ProductApp.Application.Contracts;

namespace ProductApp.Api.Endpoints.Products;

public sealed class DeleteProductEndpoint(IProductRepository repository) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/products/{id:int}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Delete an existing product.";
            s.Params["id"] = "Product identifier";
            s.Response(200, "Product deleted.");
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

        await repository.DeleteAsync(product, cancellationToken);
        await Send.OkAsync(cancellation: cancellationToken);
    }
}

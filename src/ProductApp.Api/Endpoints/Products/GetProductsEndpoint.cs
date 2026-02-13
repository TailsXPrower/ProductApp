using FastEndpoints;
using ProductApp.Api.Extensions;
using ProductApp.Application.Contracts;
using ProductApp.Model.DTO;

namespace ProductApp.Api.Endpoints.Products;

public sealed class GetProductsEndpoint(IProductRepository repository) : EndpointWithoutRequest<ProductDto[]>
{
    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Retrieve all products.";
            s.Response<ProductDto[]>(200, "Products retrieved successfully.");
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        await Send.OkAsync(products
            .Select(p => p.ToDto())
            .ToArray(), cancellationToken);
    }
}

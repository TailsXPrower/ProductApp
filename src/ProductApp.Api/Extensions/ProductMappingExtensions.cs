using ProductApp.Data.Entities;
using ProductApp.Model.DTO;
using ProductApp.Model.Requests;

namespace ProductApp.Api.Extensions;

internal static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product) =>
        new(product.Id, product.Name, product.Price, product.Description);

    public static Product ToEntity(this ProductRequest request) =>
        new(request.Name, request.Price, request.Description);

    public static void UpdateFromRequest(this Product product, ProductRequest request)
    {
        product.Name = request.Name;
        product.Price = request.Price;
        product.Description = request.Description;
    }
}

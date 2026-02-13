using ProductApp.Data.Entities;
using ProductApp.Model.DTO;

namespace ProductApp.Api.Extensions;

internal static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product) =>
        new(product.Id, product.Name, product.Price, product.Description);
}

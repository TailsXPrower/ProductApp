using ProductApp.Model.DTO;
using ProductApp.Model.Requests;

namespace ProductApp.Web.Services;

public sealed class ProductApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync("products", cancellationToken);
        await EnsureSuccess(response, cancellationToken);

        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>(cancellationToken: cancellationToken);
        return products ?? [];
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("products", request, cancellationToken);
        await EnsureSuccess(response, cancellationToken);

        var created = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken: cancellationToken);
        return created ?? throw new InvalidOperationException("Failed to deserialize created product.");
    }

    public async Task<ProductDto> UpdateProductAsync(UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync($"products/{request.Id}", request, cancellationToken);
        await EnsureSuccess(response, cancellationToken);

        var updated = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken: cancellationToken);
        return updated ?? throw new InvalidOperationException("Failed to deserialize updated product.");
    }

    public async Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"products/{id}", cancellationToken);
        await EnsureSuccess(response, cancellationToken);
    }

    private static async Task EnsureSuccess(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var message = content switch
        {
            { Length: > 0 } => content,
            _ => response.ReasonPhrase ?? "Request failed"
        };

        throw new HttpRequestException(message, null, response.StatusCode);
    }
}

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProductApp.IntegrationTests.Infrastructure;
using ProductApp.Model.DTO;
using ProductApp.Model.Requests;

namespace ProductApp.IntegrationTests;

public class ProductEndpointsTests(ProductApiFactory factory) : IClassFixture<ProductApiFactory>
{
    private readonly HttpClient _client = factory.CreateClientWithJson();

    [Fact]
    public async Task CreateProduct_ReturnsCreatedProduct()
    {
        var request = new CreateProductRequest("Test Product", 99.99m, "Test Description");

        var response = await _client.PostAsJsonAsync("/products", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        product.Should().NotBeNull();
        product.Name.Should().Be(request.Name);
        product.Price.Should().Be(request.Price);
        product.Description.Should().Be(request.Description);

        var location = response.Headers.Location;
        location.Should().NotBeNull();

        var getResponse = await _client.GetAsync(location);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsProducts()
    {
        await _client.PostAsJsonAsync("/products", new CreateProductRequest("Product1", 10m, null));
        await _client.PostAsJsonAsync("/products", new CreateProductRequest("Product2", 20m, "Desc"));

        var response = await _client.GetAsync("/products");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var products = await response.Content.ReadFromJsonAsync<ProductDto[]>();
        products.Should().NotBeNull();
        products.Length.Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetProductById_ReturnsProduct()
    {
        var createResponse = await _client.PostAsJsonAsync("/products", new CreateProductRequest("Single", 30m, null));
        var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        var response = await _client.GetAsync($"/products/{created!.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        product.Should().NotBeNull();
        product.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsUpdatedProduct()
    {
        var createResponse = await _client.PostAsJsonAsync("/products", new CreateProductRequest("Old", 40m, null));
        var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        var updateRequest = new UpdateProductRequest(created!.Id, "Updated", 50m, "Updated Desc");
        var response = await _client.PutAsJsonAsync($"/products/{created.Id}", updateRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<ProductDto>();
        updated.Should().NotBeNull();
        updated.Name.Should().Be(updateRequest.Name);
        updated.Price.Should().Be(updateRequest.Price);
        updated.Description.Should().Be(updateRequest.Description);
    }

    [Fact]
    public async Task DeleteProduct_RemovesProduct()
    {
        var createResponse = await _client.PostAsJsonAsync("/products", new CreateProductRequest("ToDelete", 60m, null));
        var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        var response = await _client.DeleteAsync($"/products/{created!.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await _client.GetAsync($"/products/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

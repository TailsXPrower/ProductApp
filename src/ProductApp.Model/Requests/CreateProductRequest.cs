namespace ProductApp.Model.Requests;

public sealed record CreateProductRequest(string Name, decimal Price, string? Description);

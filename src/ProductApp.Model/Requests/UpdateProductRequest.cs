namespace ProductApp.Model.Requests;

public sealed record UpdateProductRequest(int Id, string Name, decimal Price, string? Description);

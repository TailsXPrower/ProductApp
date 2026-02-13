namespace ProductApp.Model.Requests;

public sealed record ProductRequest(string Name, decimal Price, string? Description);

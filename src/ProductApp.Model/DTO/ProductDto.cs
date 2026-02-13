namespace ProductApp.Model.DTO;

public sealed record ProductDto(int Id, string Name, decimal Price, string? Description);

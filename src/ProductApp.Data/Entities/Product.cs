namespace ProductApp.Data.Entities;

public sealed class Product(int id, string name, decimal price, string? description)
{
    public Product(string name, decimal price, string? description) : this(0, name, price, description) {}

    public int Id { get; private set; } = id;
    public string Name { get; private set; } = ValidateName(name);
    public decimal Price { get; private set; } = ValidatePrice(price);
    public string? Description { get; private set; } = description;

    public void UpdateDetails(string name, decimal price, string? description)
    {
        Name = ValidateName(name);
        Price = ValidatePrice(price);
        Description = description;
    }

    private static string ValidateName(string name) => string.IsNullOrWhiteSpace(name)
        ? throw new ArgumentException("Product name cannot be empty.", nameof(name))
        : name;

    private static decimal ValidatePrice(decimal price) => price < 0
        ? throw new ArgumentOutOfRangeException(nameof(price), price, "Product price cannot be negative.")
        : price;
}

namespace ProductApp.Model.Forms;

public class EditProductModel
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
}
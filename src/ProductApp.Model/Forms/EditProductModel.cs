namespace ProductApp.Model.Forms;

public class EditProductModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public bool IsNew => Id == 0;
}
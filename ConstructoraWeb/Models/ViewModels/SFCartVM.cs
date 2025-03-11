namespace ConstructoraWeb.Models.ViewModels;

public class SFCartVM
{
    public long id_carrito { get; set; } = 0;
    public long id_marca { get; set; } = 0;
    public int id_producto { get; set; } = 0;
    public int cantidad { get; set; } = 0;
    public decimal subtotal { get; set; } = 0;
    public string email_usuario { get; set; } = "";
}

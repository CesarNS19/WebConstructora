namespace ConstructoraWeb.Models.ViewModels;

public class SFCartVM
{
    public long id_carrito { get; set; } = 0;
    public long id_marca { get; set; } = 0;
    public int id_producto { get; set; } = 0;
    public int cantidad { get; set; } = 0;
    public decimal subtotal { get; set; } = 0;
    public decimal precio { get; set; } = 0;
    public string email_usuario { get; set; } = "";
    public string nombre_marca { get; set; } = "";
    public string nombre { get; set; } = "";
    public string tipo_fragancia { get; set; } = "";
    public string genero { get; set; } = "";
    public int tamanio_ml { get; set; } = 0;
    public string descripcion { get; set; } = "";
    public string imagen { get; set; } = "";
}

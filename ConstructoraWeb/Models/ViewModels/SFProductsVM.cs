namespace ConstructoraWeb.Models.ViewModels;

public class SFProductsVM
{
    public int id_producto { get; set; } = 0;
    public long id_marca { get; set; } = 0;
    public string nombre { get; set; } = "";
    public string tipo_fragancia { get; set; } = "";
    public string genero { get; set; } = "";
    public int tamanio_ml { get; set; } = 0;
    public decimal precio { get; set; } = 0;
    public int stock { get; set; } = 0;
    public string descripcion { get; set; } = "";
    public string imagen { get; set; } = "";
    public string estatus { get; set; } = "";
}
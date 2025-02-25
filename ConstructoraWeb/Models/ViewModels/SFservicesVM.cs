namespace ConstructoraWeb.Models.ViewModels;

public class SFservicesVM
{
    public long MeterialID { get; set; } = 0;
    public long TypeMeterialID { get; set; } = 0;
    public string MeterialName { get; set; } = "";
    public float MeterialWeight { get; set; } = 0;
    public float MeterialX { get; set; } = 0;
    public float MeterialY { get; set; } = 0;
    public float MeterialZ { get; set; } = 0;
    public string MeterialSTS { get; set; } = "";
    public string MeterialImage { get; set; } = "";
    public string TypeMeterialName { get; set; } = "";
    public string TypeMeterialSTS { get; set; } = "";
}
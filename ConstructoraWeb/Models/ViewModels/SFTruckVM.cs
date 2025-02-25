namespace ConstructoraWeb.Models.ViewModels;

public class SFTruckVM
{
    public long TruckID { get; set; } = 0;
    public long TypeID { get; set; } = 0;
    public string TruckName { get; set; } = "";
    public float TruckSupportWeight { get; set; } = 0;
    public float TruckX { get; set; } = 0;
    public float TruckY { get; set; } = 0;
    public float TruckZ { get; set; } = 0;
    public string TruckSTS { get; set; } = "";
    public string TruckImage { get; set; } = "";
}

namespace ConstructoraWeb.Models.ViewModels
{
  public class SubModuleVM
  {
    public long SubModuleID { get; set; } = 0;
    public string SubDescription { get; set; } = "";
    public string SubAction { get; set; } = "";
    public string SubStatus { get; set; } = "";
    public int SubPosition { get; set; } = 0;
    public DateTime? SubRDate { get; set; }
    public long ModuleID { get; set; } = 0;
    public string MoDescription { get; set; } = "";
    public string MoController { get; set; } = "";
    public string MoIcon { get; set; } = "";
    public string MoStatus { get; set; } = "";
    public int MoPosition { get; set; } = 0;
    public DateTime? MoRDate { get; set; }
    public long auxID { get; set; }
    public int IsPermissionAply { get; set; }
    public int PermissionID { get; set; } = 0;
    public string PerStatus { get; set; } = "";
    public string AcEmailAddress { get; set; } = "";
   
  }
}
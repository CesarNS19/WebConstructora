namespace ConstructoraWeb.Models.ViewModels
{
    public class ModuleVM
    {
        public long ModuleID { get; set; } = 0;
        public string MoDescription { get; set; } = "";
        public string MoController { get; set; } = "";
        public string MoIcon { get; set; } = "";
        public string MoStatus { get; set; } = "";
        public int MoPosition { get; set; } = 0;
        public DateTime? MoRDate { get; set; }

    }
}
namespace ConstructoraWeb.Models.ViewModels
{
    public class PermissionVM
    {
        public long PermissionID { get; set; } = 0;
        public string PerStatus { get; set; } = "";
        public DateTime? SubRDate { get; set; }
        public long SubModuleID { get; set; } = 0;
        public long ProfileID { get; set; } = 0;
    }
}
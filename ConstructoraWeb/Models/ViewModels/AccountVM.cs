namespace ConstructoraWeb.Models.ViewModels
{
    public class AccountVM
    {
        public int AccountID { get; set; } = 0;
        public string AcUser { get; set; } = "";
        public string AcPassword { get; set; } = "";
        public string AcEmailAddress { get; set; } = "";
        public string AcPhoneNumber { get; set; } = "";
        public int AcVerifyEmail { get; set; } = 0;
        public string AcStatus { get; set; } = "";
        public string AcRDate { get; set; } = "";
        public int PeopleID { get; set; } = 0;
        public string PeFirstName { get; set; } = "";
        public string PeMiddleInitial { get; set; } = "";
        public string PeLastName { get; set; } = "";
        public string PeStatus { get; set; } = "";
        public string PeRDate { get; set; } = "";
        public int ProfileID { get; set; } = 0;
        public string ProDescription { get; set; } = "";
        public string ProStatus { get; set; } = "";
        public string ProRDate { get; set; } = "";
        public string PeStreet { get; set; } = "";
        public string PeOutsideCode { get; set; } = "";
        public string PeInsideCode { get; set; } = "";
        public int SettlementID { get; set; } = 0;
    }
}
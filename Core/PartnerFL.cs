using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class PartnerFL: Partner
    {
        public string INN { get; set; } = null!;
        public string PassportSeries { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PassportDateOfIssue { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PassportDateOfBirth { get; set; }
        public string PassportPlaseOfIssue { get; set; } = null!;
        public string PassportDivisionCode { get; set; } = null!;
        public List<Employee>? Employeers { get; set; } = null!;
    }
}

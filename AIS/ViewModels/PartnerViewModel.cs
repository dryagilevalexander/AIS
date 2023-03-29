using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels
{
    public class PartnerViewModel
    {
        public int Id { get; set; }
        public int PartnerTypeId { get; set; }
        public IEnumerable<SelectListItem>? PartnerTypes { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? INN { get; set; }
        public string? KPP { get; set; }
        public string? OGRN { get; set; }
        public string? Bank { get; set; }
        public string? Account { get; set; }
        public string? CorrespondentAccount { get; set; }
        public string? BIK { get; set; }
        public int? DirectorTypeId { get; set; }
        public IEnumerable<SelectListItem>? DirectorTypes { get; set; }
        public IEnumerable<SelectListItem>? PartnerStatuses { get; set; }
        public String? DirectorName { get; set; }
        public int? PartnerStatusId { get; set; }
        public List<Employee>? Employeers { get; set; }
        public List<Contract>? Contracts { get; set; }
        public string? PassportSeries { get; set; }
        public string? PassportNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportDateOfIssue { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportDateOfBirth { get; set; }
        public string? PassportPlaseOfIssue { get; set; }
        public string? PassportDivisionCode { get; set; }
    }


}


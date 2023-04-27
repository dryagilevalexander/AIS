using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels
{
    public class MyContractViewModel
    {
        public int Id { get; set; }
        public string? NumberOfContract { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)] 
        public DateTime? DateStart { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }
        public IEnumerable<SelectListItem>? MyPartners { get; set; }
        [Required(ErrorMessage = "Не указан контрагент")]
        public int? PartnerId { get; set; }
        [Required(ErrorMessage = "Не указан предмет контракта")]
        public string? SubjectOfContract { get; set; }
        public decimal Cost { get; set; }
        public string? ProjectContractLink { get; set; }
        public TypeOfContract TypeOfContract { get; set; }
        public IEnumerable<MyFile>? MyFiles { get; set; }
        public List<MyFile>? ContractFiles { get; set; }
        public IFormFileCollection? Enclosure { get; set; }
        public IEnumerable<SelectListItem>? TypeOfContracts { get; set; }
        public int TypeOfStateRegId { get; set; }
        public IEnumerable<SelectListItem>? TypeOfStateRegs { get; set; }
        public int? ArticleOfLawId { get; set; }
        public IEnumerable<SelectListItem>? ArticleOfLaws { get; set; }
        public int? MyContractStatusId { get; set; }
        public MyContractStatus? MyContractStatus { get; set; }
        public IEnumerable<SelectListItem>? MyContractStatuses { get; set; }
        public bool IsCustomer { get; set; }
        public string PlaceOfContract { get; set; }
        public int ContractTemplateId { get; set; }
        public IEnumerable<SelectListItem>? ContractTemplates { get; set; }

    }
}



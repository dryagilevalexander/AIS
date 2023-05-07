using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels
{
    public class DocumentTemplateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано наименование шаблона")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано описание шаблона")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Не указан тип контракта")]
        public int? TypeOfContractId { get; set; }
        public int RootTemplateId { get; set; }
        public int TypeOfDocumentId { get; set; }
        public IEnumerable<SelectListItem>? TypesOfContract { get; set; }
        public IEnumerable<SelectListItem>? TypesOfStateReg { get; set; }
        public IEnumerable<SelectListItem>? TypesOfCondition { get; set; }
        public List<Condition>? Conditions { get; set; }
    }
}

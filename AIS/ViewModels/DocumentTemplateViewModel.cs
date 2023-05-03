using Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIS.ViewModels
{
    public class DocumentTemplateViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? TypeOfContractId { get; set; }
        public int RootTemplateId { get; set; }
        public int TypeOfDocumentId { get; set; }
        public IEnumerable<SelectListItem>? TypesOfContract { get; set; }
        public IEnumerable<SelectListItem>? TypesOfStateReg { get; set; }
        public IEnumerable<SelectListItem>? TypesOfCondition { get; set; }
        public List<Condition> Conditions { get; set; }
    }
}

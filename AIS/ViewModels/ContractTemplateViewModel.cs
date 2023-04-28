using Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIS.ViewModels
{
    public class ContractTemplateViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int TypeOfContractId { get; set; }
        public int CommonContractTemplateId { get; set; }
        public IEnumerable<SelectListItem>? TypesOfContract { get; set; }
        public IEnumerable<SelectListItem>? TypesOfStateReg { get; set; }
        public IEnumerable<SelectListItem>? TypesOfCondition { get; set; }
        public List<Condition> Conditions { get; set; }
    }
}

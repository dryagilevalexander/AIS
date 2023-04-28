using Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIS.ViewModels
{
    public class CommonContractTemplateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Preamble { get; set; }
        public List<ContractTemplate> ContractTemplates { get; set; }
    }
}
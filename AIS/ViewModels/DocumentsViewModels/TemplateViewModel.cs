using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class TemplateViewModel
    {
        public int Id { get; set; }
        public string NameOfTemplate { get; set; }
        public string OutputName { get; set; }
        public int? TypeOfContractId { get; set; }
        public IEnumerable<SelectListItem>? TypesOfContract { get; set; }
        public List<int> TypeOfStateRegIds { get; set; }
        public IEnumerable<SelectListItem>? TypeOfStateRegs { get; set; }
        public List<int> PartnerTypeIds { get; set; }
        public IEnumerable<SelectListItem>? PartnerTypes { get; set; }
        public MyFile TemplateFile { get; set; }
        public IFormFile Enclosure { get; set; }
    }
}



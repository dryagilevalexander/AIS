using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class CreateRootTemplateViewModel
    {
        [Required(ErrorMessage = "Не указано наименование шаблона")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано описание шаблона")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Не указан тип документа")]
        public int TypeOfDocumentId { get; set; }
        public IEnumerable<SelectListItem>? TypesOfDocument { get; set; }
        public List<DocumentTemplate>? DocumentTemplates { get; set; }

        public async Task Fill(IContractsService _contractsService)
        {
            var documentTypes = await _contractsService.GetTypesOfDocument();
            TypesOfDocument = from documentType in documentTypes select new SelectListItem { Text = documentType.Name, Value = documentType.Id.ToString() };
        }
    }
}
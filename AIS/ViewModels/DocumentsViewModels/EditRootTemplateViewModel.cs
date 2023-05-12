using AIS.ErrorManager;
using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class EditRootTemplateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано наименование шаблона")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано описание шаблона")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Не указан тип документа")]
        public int TypeOfDocumentId { get; set; }
        public IEnumerable<SelectListItem>? TypesOfDocument { get; set; }
        public List<DocumentTemplate>? DocumentTemplates { get; set; }

        public async Task Fill(int id, IConditionsService _conditionsService, IContractsService _contractsService)
        {
            var documentTypes = await _contractsService.GetTypesOfDocument();

            RootTemplate? rootTemplate = await _conditionsService.GetRootTemplateWithDocumentTemplatesById(id);
            if (rootTemplate == null) throw new AisException("Не найдет корневой шаблон документа", HttpStatusCode.BadRequest);

            Id = rootTemplate.Id;
            Name = rootTemplate.Name;
            Description = rootTemplate.Description;
            DocumentTemplates = rootTemplate.DocumentTemplates;
            TypeOfDocumentId = rootTemplate.TypeOfDocumentId;

            TypesOfDocument = from documentType in documentTypes select new SelectListItem { Text = documentType.Name, Value = documentType.Id.ToString() };
        }
    }
}
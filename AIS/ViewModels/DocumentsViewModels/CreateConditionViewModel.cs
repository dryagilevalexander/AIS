using AIS.ErrorManager;
using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class CreateConditionViewModel
    {
        [Required(ErrorMessage = "Не указано наименование условия")]
        public string Name { get; set; } = null!;
        public string? Title { get; set; }
        public int TypeOfDocumentId { get; set; }
        public int NumLevelReference { get; set; }
        public int NumId { get; set; }
        [Required(ErrorMessage = "Не указан тип выравнивания текста")]
        public string Justification { get; set; }
        public IEnumerable<SelectListItem>? TypesOfCondition { get; set; }
        public IEnumerable<SelectListItem>? TypesOfStateReg { get; set; }
        public int DocumentTemplateId { get; set; }
        public int TypeOfConditionId { get; set; }
        public int? TypeOfStateRegId { get; set; }
        public List<SubCondition>? SubConditions { get; set; }

        public async Task Fill(int id, IConditionsService _conditionsService, IContractsService _contractsService)
        {
            DocumentTemplate? documentTemplate = await _conditionsService.GetDocumentTemplateWithRootTemplateById(id);
            if (documentTemplate == null) throw new AisException("Не найден шаблон документа", HttpStatusCode.BadRequest);
            int typeOfDocumentId = documentTemplate.RootTemplate.TypeOfDocumentId;
            DocumentTemplateId = id;
            TypeOfDocumentId = typeOfDocumentId;
            var typesOfStateReg = await _contractsService.GetTypeOfStateRegs();
            TypesOfStateReg = from typeOfStateReg in typesOfStateReg select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
        }
    }
}
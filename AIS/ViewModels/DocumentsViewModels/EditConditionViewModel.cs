using AIS.ErrorManager;
using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class EditConditionViewModel
    {
        public int? Id { get; set; }
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
            Infrastructure.Models.Condition? condition = await _conditionsService.GetCondition(id);
            if (condition == null) throw new AisException("Не найден пункт шаблона документа", HttpStatusCode.BadRequest);
            DocumentTemplate? documentTemplate = await _conditionsService.GetDocumentTemplateWithRootTemplateById(condition.DocumentTemplateId);
            if (documentTemplate == null) throw new AisException("Не найден шаблон документа", HttpStatusCode.BadRequest);
            Id = condition.Id;
            if (condition.TypeOfStateRegId != null) TypeOfStateRegId = condition.TypeOfStateRegId.Value;
            Title = condition.Title;
            Name = condition.Name;
            DocumentTemplateId = condition.DocumentTemplateId;
            int typeOfDocumentId = documentTemplate.RootTemplate.TypeOfDocumentId;
            TypeOfDocumentId = typeOfDocumentId;
            NumLevelReference = condition.NumLevelReference;
            NumId = condition.NumId;
            Justification = condition.Justification;
            SubConditions = condition.SubConditions;
            var typesOfStateReg = await _contractsService.GetTypeOfStateRegs();
            TypesOfStateReg = from typeOfStateReg in typesOfStateReg select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
        }
    }
}
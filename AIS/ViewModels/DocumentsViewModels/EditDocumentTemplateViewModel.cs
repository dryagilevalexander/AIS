using AIS.ErrorManager;
using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class EditDocumentTemplateViewModel
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

        public async Task Fill(int id, IConditionsService _conditionsService, IContractsService _contractsService)
        {
            DocumentTemplate? documentTemplate = await _conditionsService.GetDocumentTemplateWithConditionsById(id);
            if (documentTemplate == null) throw new AisException("Не найден шаблон документа", HttpStatusCode.BadRequest);

            var typesOfContract = await _contractsService.GetTypeOfContracts();
            var typesOfStateReg = await _contractsService.GetTypeOfStateRegs();
            var conditions = documentTemplate.Conditions;
            conditions = conditions.OrderBy(x => x.NumberInDocument).ToList<Infrastructure.Models.Condition>();

            Id = documentTemplate.Id;
            Name = documentTemplate.Name;
            Description = documentTemplate.Description;
            Conditions = conditions;

            if (documentTemplate.TypeOfContractId != null) TypeOfContractId = documentTemplate.TypeOfContractId.Value;

            RootTemplateId = documentTemplate.RootTemplateId;
            TypesOfContract = from typeOfContract in typesOfContract select new SelectListItem { Text = typeOfContract.Name, Value = typeOfContract.Id.ToString() };
            TypesOfStateReg = from typeOfStateReg in typesOfStateReg select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
        }
    }
}

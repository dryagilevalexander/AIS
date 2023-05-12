using AIS.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class CreateDocumentTemplateViewModel
    {
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
            RootTemplateId = id;
            RootTemplate rootTemplate = await _conditionsService.GetRootTemplateById(id);
            TypeOfDocumentId = rootTemplate.TypeOfDocumentId;
            var typesOfContract = await _contractsService.GetTypeOfContracts();
            TypesOfContract = from typeOfContract in typesOfContract select new SelectListItem { Text = typeOfContract.Name, Value = typeOfContract.Id.ToString() };
        }
    }
}

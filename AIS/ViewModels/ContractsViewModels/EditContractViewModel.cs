using AIS.ErrorManager;
using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.ContractsViewModels
{
    public class EditContractViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указан номер контракта")]
        public string NumberOfContract { get; set; }
        [Required(ErrorMessage = "Не указана дата заключения контракта")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateStart { get; set; }
        [Required(ErrorMessage = "Не указана дата завершения исполнения контракта")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }
        public IEnumerable<SelectListItem>? MyPartners { get; set; }
        [Required(ErrorMessage = "Не указан контрагент")]
        public int? PartnerOrganizationId { get; set; }
        [Required(ErrorMessage = "Не указан предмет контракта")]
        public string? SubjectOfContract { get; set; }
        [Required(ErrorMessage = "Не указана сумма контракта")]
        public decimal Cost { get; set; }
        public string? ProjectContractLink { get; set; }
        public IEnumerable<MyFile>? MyFiles { get; set; }
        public List<MyFile>? ContractFiles { get; set; }
        public IFormFileCollection? Enclosure { get; set; }

        [Required(ErrorMessage = "Не указан тип государственного регулирования")]
        public int TypeOfStateRegId { get; set; }
        public IEnumerable<SelectListItem>? TypeOfStateRegs { get; set; }
        public int? ArticleOfLawId { get; set; }
        public IEnumerable<SelectListItem>? ArticleOfLaws { get; set; }
        [Required(ErrorMessage = "Не указан статус контракта")]
        public int? MyContractStatusId { get; set; }
        public MyContractStatus? MyContractStatus { get; set; }
        public IEnumerable<SelectListItem>? MyContractStatuses { get; set; }
        [Required(ErrorMessage = "Не указана роль организации")]
        public bool IsCustomer { get; set; }
        [Required(ErrorMessage = "Не указано место заключения контракта")]
        public string PlaceOfContract { get; set; }
        [Required(ErrorMessage = "Не указан шаблон контракта")]
        public int DocumentTemplateId { get; set; }
        public IEnumerable<SelectListItem>? DocumentTemplates { get; set; }

        public async Task Fill(int id, IContractsService _contractsService, IEnclosureService _enclosureService, IPartnerService _partnerService)
        {
            Contract? contract = await _contractsService.GetContractByIdWithMyFiles(id);
            if (contract == null) throw new AisException("Договор не найден", HttpStatusCode.BadRequest);

            IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresByContractId(id);
            IEnumerable<Partner> myPartners = await _partnerService.GetPartners();
            var partners = from myPartner in myPartners select new SelectListItem { Text = myPartner.ShortName, Value = myPartner.Id.ToString() };
            IEnumerable<MyContractStatus> myContractStatuses = await _contractsService.GetMyContractStatuses();
            var contractStatuses = from statusOfContract in myContractStatuses select new SelectListItem { Text = statusOfContract.Name, Value = statusOfContract.Id.ToString() };
            IEnumerable<TypeOfStateReg> typeOfStateRegs = await _contractsService.GetTypeOfStateRegs();
            IEnumerable<ArticleOfLaw> articleOfLaws = await _contractsService.GetArticleOfLaws();

            Id = contract.Id;
            NumberOfContract = contract.NumberOfContract;
            DateStart = contract.DateStart;
            DateEnd = contract.DateEnd;
            PartnerOrganizationId = contract.PartnerOrganizationId;
            SubjectOfContract = contract.SubjectOfContract;
            Cost = (decimal)contract.Cost;
            MyPartners = partners;
            MyContractStatuses = contractStatuses;
            MyContractStatusId = contract.MyContractStatusId;
            MyFiles = enclosures;
            TypeOfStateRegs = from typeOfStateReg in typeOfStateRegs select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
            ArticleOfLaws = from articleOfLaw in articleOfLaws select new SelectListItem { Text = articleOfLaw.Name, Value = articleOfLaw.Id.ToString() };
        }

    }
}



﻿using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.ContractsViewModels
{
    public class CreateContractViewModel
    {
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

        public async Task Fill(IPartnerService _partnerService, IConditionsService _conditionsService, IContractsService _contractsService)
        {
            IEnumerable<Partner> myPartners = await _partnerService.GetPartnersWithoutOurOrganization();
            IEnumerable<DocumentTemplate> documentTemplates = await _conditionsService.GetDocumentTemplatesByTypeOfDocumentId(1);
            IEnumerable<TypeOfStateReg> typeOfStateRegs = await _contractsService.GetTypeOfStateRegs();
            IEnumerable<ArticleOfLaw> articleOfLaws = await _contractsService.GetArticleOfLaws();
            IEnumerable<MyContractStatus> myContractStatuses = await _contractsService.GetMyContractStatuses();
            MyPartners = from myPartner in myPartners select new SelectListItem { Text = myPartner.ShortName, Value = myPartner.Id.ToString() };
            DocumentTemplates = from documentTemplate in documentTemplates select new SelectListItem { Text = documentTemplate.Name, Value = documentTemplate.Id.ToString() };
            TypeOfStateRegs = from typeOfStateReg in typeOfStateRegs select new SelectListItem { Text = typeOfStateReg.Name, Value = typeOfStateReg.Id.ToString() };
            ArticleOfLaws = from articleOfLaw in articleOfLaws select new SelectListItem { Text = articleOfLaw.Name, Value = articleOfLaw.Id.ToString() };
            MyContractStatuses = from statusOfContract in myContractStatuses select new SelectListItem { Text = statusOfContract.Name, Value = statusOfContract.Id.ToString() };
        }
    }
}



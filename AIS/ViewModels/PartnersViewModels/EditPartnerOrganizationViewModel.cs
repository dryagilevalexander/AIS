using AIS.ErrorManager;
using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.PartnersViewModels
{
    public class EditPartnerOrganizationViewModel
    {
        public int Id { get; set; }
        public int PartnerTypeId { get; set; }
        [Required(ErrorMessage = "Не указана категория контрагента")]
        public int PartnerCategoryId { get; set; }
        [Required(ErrorMessage = "Не указано наименование контрагента")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано короткое наименование контрагента")]
        public string ShortName { get; set; }
        [Required(ErrorMessage = "Не указан адрес контрагента")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Не указан email контрагента")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Не указан телефонный номер контрагента")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Не указан ИНН контрагента")]
        public string INN { get; set; }
        [Required(ErrorMessage = "Не указан КПП контрагента")]
        public string KPP { get; set; }
        [Required(ErrorMessage = "Не указан ОГРН контрагента")]
        public string OGRN { get; set; }
        [Required(ErrorMessage = "Не указан банк контрагента")]
        public string Bank { get; set; }
        [Required(ErrorMessage = "Не указан расчетный счет контрагента")]
        public string Account { get; set; }
        [Required(ErrorMessage = "Не указан корреспондентский счет контрагента")]
        public string CorrespondentAccount { get; set; }
        [Required(ErrorMessage = "Не указан БИК банка контрагента")]
        public string BIK { get; set; }
        [Required(ErrorMessage = "Не указан тип руководителя контрагента")]
        public int DirectorTypeId { get; set; }
        public IEnumerable<SelectListItem>? DirectorTypes { get; set; }
        public IEnumerable<SelectListItem>? PartnerStatuses { get; set; }
        public IEnumerable<SelectListItem>? PartnerCategories { get; set; }
        [Required(ErrorMessage = "Не указано ФИО руководителя контрагента")]
        public string DirectorName { get; set; }
        [Required(ErrorMessage = "Не указано ФИО руководителя контрагента")]
        public string DirectorNameR { get; set; }
        [Required(ErrorMessage = "Не указан статус контрагента")]
        public int PartnerStatusId { get; set; }
        public List<Employee>? Employees { get; set; }

        public async Task Fill(int id, IPartnerService _partnerService)
        {
            Partner? partner = await _partnerService.GetPartner(id);
            if(partner == null) throw new AisException("Не найден контрагент", HttpStatusCode.BadRequest);
            List<Employee> employees = await _partnerService.GetEmployeesByPartnerId(id);

            Id = partner.Id;
            Name = partner.Name;
            ShortName = partner.ShortName;
            INN = partner.INN;
            KPP = partner.KPP;
            DirectorTypeId = partner.DirectorTypeId.Value;
            DirectorName = partner.DirectorName;
            DirectorNameR = partner.DirectorNameR;
            Bank = partner.Bank;
            Account = partner.Account;
            CorrespondentAccount = partner.CorrespondentAccount;
            BIK = partner.BIK;
            OGRN = partner.OGRN;
            PartnerCategoryId = partner.PartnerCategoryId.Value;
            PartnerTypeId = partner.PartnerTypeId;
            Address = partner.Address;
            PhoneNumber = partner.PhoneNumber;
            Email = partner.Email;
            Employees = employees;

            var directorTypes = await _partnerService.GetDirectorTypes();
            var categories = await _partnerService.GetCategories();
            var partnerStatuses = await _partnerService.GetPartnerStatuses();

            DirectorTypes = from directorType in directorTypes select new SelectListItem { Text = directorType.Name, Value = directorType.Id.ToString() };
            PartnerCategories = from category in categories select new SelectListItem { Text = category.Name, Value = category.Id.ToString() };
            DirectorTypes = from directorType in directorTypes select new SelectListItem { Text = directorType.Name, Value = directorType.Id.ToString() };
            PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };
            PartnerTypeId = 1;
        }
    }
}


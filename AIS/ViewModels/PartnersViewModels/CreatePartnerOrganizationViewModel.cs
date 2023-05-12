using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.PartnersViewModels
{
    public class CreatePartnerOrganizationViewModel
    {
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
    }


}


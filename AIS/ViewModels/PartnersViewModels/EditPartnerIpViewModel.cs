﻿using AIS.ErrorManager;
using AIS.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.PartnersViewModels
{
    public class EditPartnerIpViewModel
    {
        public int Id { get; set; }
        public int PartnerTypeId { get; set; }
        [Required(ErrorMessage = "Не указано ФИО контрагента")]
        public string? Fio { get; set; }
        [Required(ErrorMessage = "Не указано ФИО с инициалами контрагента")]
        public string ShortFio { get; set; }
        [Required(ErrorMessage = "Не указано ФИО с инициалами контрагента")]
        public string ShortFioR { get; set; }
        [Required(ErrorMessage = "Не указан адрес контрагента")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Не указан email контрагента")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Не указан телефонный номер контрагента")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Не указан ИНН контрагента")]
        public string? INN { get; set; }
        [Required(ErrorMessage = "Не указан статус контрагента")]
        public int? PartnerStatusId { get; set; }
        public PartnerStatus? PartnerStatus { get; set; }
        [Required(ErrorMessage = "Не указан банк контрагента")]
        public string? Bank { get; set; }
        [Required(ErrorMessage = "Не указан расчетный счет контрагента")]
        public string? Account { get; set; }
        [Required(ErrorMessage = "Не указан корреспондетский счет контрагента")]
        public string? CorrespondentAccount { get; set; }
        [Required(ErrorMessage = "Не указан БИК банка контрагента")]
        public string? BIK { get; set; }
        [Required(ErrorMessage = "Не указана серия паспорта контрагента")]
        public string? PassportSeries { get; set; }
        [Required(ErrorMessage = "Не указан номер паспорта контрагента")]
        public string? PassportNumber { get; set; }
        [Required(ErrorMessage = "Не указана дата выдачи паспорта")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportDateOfIssue { get; set; }
        [Required(ErrorMessage = "Не указана дата рождения")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportDateOfBirth { get; set; }
        [Required(ErrorMessage = "Не указано место выдачи паспорта")]
        public string? PassportPlaseOfIssue { get; set; }
        [Required(ErrorMessage = "Не указан код подразделения")]
        public string? PassportDivisionCode { get; set; }
        public IEnumerable<SelectListItem>? PartnerStatuses { get; set; }
    
        public async Task Fill(int id, IPartnerService _partnerService)
        {
            Partner? partner = await _partnerService.GetPartner(id);
            if (partner == null) throw new AisException("Не найден контрагент", HttpStatusCode.BadRequest);

            Id = partner.Id;
            Fio = partner.Fio;
            ShortFio = partner.ShortFio;
            ShortFioR = partner.ShortFioR;
            Address = partner.Address;
            Email = partner.Email;
            PhoneNumber = partner.PhoneNumber;
            INN = partner.INN;
            PartnerStatusId = partner.PartnerStatusId;
            Bank = partner.Bank;
            Account = partner.Account;
            CorrespondentAccount = partner.CorrespondentAccount;
            BIK = partner.BIK;
            PassportSeries = partner.PassportSeries;
            PassportNumber = partner.PassportNumber;
            PassportDateOfIssue = partner.PassportDateOfIssue;
            PassportDateOfBirth = partner.PassportDateOfBirth;
            PassportPlaseOfIssue = partner.PassportPlaseOfIssue;
            PassportDivisionCode = partner.PassportDivisionCode;

            var partnerStatuses = await _partnerService.GetPartnerStatuses();
            PartnerStatuses = from partnerStatus in partnerStatuses select new SelectListItem { Text = partnerStatus.Name, Value = partnerStatus.Id.ToString() };
            PartnerTypeId = 2;
        }
    }
}

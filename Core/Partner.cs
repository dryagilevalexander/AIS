﻿using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace Core
{
    public class Partner
    {
        //Общее
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? INN {get; set; }
        public int? PartnerStatusId { get; set; }
        public PartnerStatus? PartnerStatus { get; set; }
        public int PartnerTypeId { get; set; }
        public PartnerType? PartnerType { get; set; }
        public List<Contract>? Contracts { get; set; }
        //ЮЛ
        public string? KPP { get; set; }
        public int? DirectorTypeId { get; set; }
        public DirectorType? DirectorType { get; set; }
        public string? DirectorName { get; set; }
        //ЮЛ ИП
        public string? Bank { get; set; }
        public string? Account { get; set; }
        public string? CorrespondentAccount { get; set; }
        public string? BIK { get; set; }
        public List<Employee>? Employeers { get; set; }
        public string? OGRN { get; set; }
        //ФЛ
        public string? PassportSeries { get; set; }
        public string? PassportNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportDateOfIssue { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportDateOfBirth { get; set; }
        public string? PassportPlaseOfIssue { get; set; }
        public string? PassportDivisionCode { get; set; }
    }
}
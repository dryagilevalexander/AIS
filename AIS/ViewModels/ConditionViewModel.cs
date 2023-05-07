﻿using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels
{
    public class ConditionViewModel
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
    }
}
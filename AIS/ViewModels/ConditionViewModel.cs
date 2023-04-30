﻿using Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIS.ViewModels
{
    public class ConditionViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Text { get; set; }
        public int TypeOfDocumentId { get; set; }
        public int NumLevelReference { get; set; }
        public int NumId { get; set; }
        public IEnumerable<SelectListItem>? TypesOfCondition { get; set; }
        public IEnumerable<SelectListItem>? TypesOfStateReg { get; set; }
        public int ContractTemplateId { get; set; }
        public int TypeOfConditionId { get; set; }
        public int TypeOfStateRegId { get; set; }
        public List<SubCondition> SubConditions { get; set; }
    }
}
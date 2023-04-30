﻿using Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIS.ViewModels
{
    public class SubConditionParagraphViewModel
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public int NumLevelReference { get; set; }
        public int NumId { get; set; }
        public SubCondition SubCondition { get; set; }
        public int SubConditionId { get; set; }
    }
}
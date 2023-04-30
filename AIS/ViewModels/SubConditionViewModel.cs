using Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIS.ViewModels
{
    public class SubConditionViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Text { get; set; }
        public int NumLevelReference { get; set; }
        public int NumId { get; set; }
        public Condition Condition { get; set; }
        public int ConditionId { get; set; }
        public List<SubConditionParagraph>? SubConditionParagraphs { get; set; }
    }
}
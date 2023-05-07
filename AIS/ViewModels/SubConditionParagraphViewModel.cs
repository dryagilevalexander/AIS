using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels
{
    public class SubConditionParagraphViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Не указан текст параграфа")]
        public string Text { get; set; } = null!;
        public int NumLevelReference { get; set; }
        public int NumId { get; set; }
        [Required(ErrorMessage = "Не указан тип выравнивания текста")]
        public string Justification { get; set; } = null!;
        public SubCondition? SubCondition { get; set; }
        public int SubConditionId { get; set; }
    }
}
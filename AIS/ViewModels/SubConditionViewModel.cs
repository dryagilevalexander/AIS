using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels
{
    public class SubConditionViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Не указано наименование подпункта")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Не указан текст подпункта")]
        public string Text { get; set; } = null!;
        public int NumLevelReference { get; set; }
        public int NumId { get; set; }
        [Required(ErrorMessage = "Не указан тип выравнивания текста")]
        public string Justification { get; set; } = null!;
        public int ConditionId { get; set; }
        public Condition? Condition { get; set; }
        public List<SubConditionParagraph>? SubConditionParagraphs { get; set; }
    }
}
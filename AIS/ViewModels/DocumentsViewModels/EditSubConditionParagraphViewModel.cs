using AIS.ErrorManager;
using AIS.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class EditSubConditionParagraphViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указан текст параграфа")]
        public string Text { get; set; } = null!;
        public int NumLevelReference { get; set; }
        public int NumId { get; set; }
        [Required(ErrorMessage = "Не указан тип выравнивания текста")]
        public string Justification { get; set; } = null!;
        public SubCondition? SubCondition { get; set; }
        public int SubConditionId { get; set; }

        public async Task Fill(int id, IConditionsService _conditionsService)
        {
            SubConditionParagraph? subConditionParagraph = await _conditionsService.GetSubConditionParagraph(id);
            if (subConditionParagraph == null) throw new AisException("Не найден абзац", HttpStatusCode.BadRequest);
            Id = subConditionParagraph.Id;
            Text = subConditionParagraph.Text;
            NumLevelReference = subConditionParagraph.NumLevelReference;
            NumId = subConditionParagraph.NumId;
            Justification = subConditionParagraph.Justification;
            SubConditionId = subConditionParagraph.SubConditionId;
        }
    }
}
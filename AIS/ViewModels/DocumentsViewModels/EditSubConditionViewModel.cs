using AIS.ErrorManager;
using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class EditSubConditionViewModel
    {
        public int Id { get; set; }
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

        public async Task Fill(int id, IConditionsService _conditionsService)
        {
            SubCondition subCondition = await _conditionsService.GetSubCondition(id);
            if (subCondition == null) throw new AisException("Не найден подпункт", HttpStatusCode.BadRequest);
            Id = subCondition.Id;
            Name = subCondition.Name;
            Text = subCondition.Text;
            ConditionId = subCondition.ConditionId;
            NumLevelReference = subCondition.NumLevelReference;
            NumId = subCondition.NumId;
            Justification = subCondition.Justification;
            SubConditionParagraphs = subCondition.SubConditionParagraphs;
        }
    }
}
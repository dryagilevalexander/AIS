using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class RootTemplateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано наименование шаблона")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано описание шаблона")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Не указан тип документа")]
        public int TypeOfDocumentId { get; set; }
        public IEnumerable<SelectListItem>? TypesOfDocument { get; set; }
        public List<DocumentTemplate>? DocumentTemplates { get; set; }
    }
}
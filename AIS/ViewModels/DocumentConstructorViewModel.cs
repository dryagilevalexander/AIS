using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels
{
    public class DocumentConstructorViewModel
    {
       [Required(ErrorMessage = "Не указан номер договора")]
       public string NumberOfContract { get; set; }
       [Required(ErrorMessage = "Укажите тип документа на основании которого действует руководитель")]
       public string PartnerDocumentType { get; set; }
       [Required(ErrorMessage = "Не указан предмет договора")]
       public string SubjectOfContract { get; set; }
       public DateTime? DateStart { get; set; }        
       public DateTime? DateEnd { get; set; }
       [Required(ErrorMessage = "Не указана цена договора")]
       public decimal Cost { get; set; }
       [Required(ErrorMessage = "Не указан контрагент")]
       public int PartnerId { get; set; }
       public IEnumerable<SelectListItem>? MyPartners { get; set; }
       [Required(ErrorMessage = "Не указан тип государственного регулирования")]
       public int TypeOfStateRegId { get; set; }
       public IEnumerable<SelectListItem>? TypeOfStateRegs { get; set; }
       public int? ArticleOfLawId { get; set; }
       public IEnumerable<SelectListItem>? ArticleOfLaws { get; set; }
    }
}


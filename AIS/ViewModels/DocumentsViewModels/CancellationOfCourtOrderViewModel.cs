using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.DocumentsViewModels
{
    public class CancellationOfCourtOrderViewModel
    {
        public string? NumberOfСourtСase { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfOrder { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public IEnumerable<SelectListItem>? MyPartners { get; set; }
        [Required(ErrorMessage = "Не указан взыскатель")]
        public int? PartnerId { get; set; }
        public IEnumerable<SelectListItem>? Courts { get; set; }
        [Required(ErrorMessage = "Не указан судебный орган")]
        public int CourtId { get; set; }

        [Required(ErrorMessage = "Не указан предмет судебного приказа")]
        public string? SubjectOfOrder { get; set; }
    }
}



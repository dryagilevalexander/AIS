using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.ProcessViewModels
{
    public class LetterViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DepartureDate { get; set; }
        public string Name { get; set; }
        public string Destination { get; set; }
        public int ShippingMethodId { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public int LetterTypeId { get; set; }
        public LetterType LetterType { get; set; }
        public IEnumerable<SelectListItem>? ShippingMethods { get; set; }
        public IEnumerable<SelectListItem>? LetterTypes { get; set; }
    }
}

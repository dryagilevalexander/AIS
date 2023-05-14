using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.ProcessViewModels
{
    public class CreateLetterViewModel
    {
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

        public async Task Fill(ILetterService _letterService)
        {
            var shippingMethods = await _letterService.GetAllShippingMethods();
            ShippingMethods = from shippingMethod in shippingMethods select new SelectListItem { Text = shippingMethod.Name, Value = shippingMethod.Id.ToString() };
            var letterTypes = await _letterService.GetAllletterTypes();
            LetterTypes = from letterType in letterTypes select new SelectListItem { Text = letterType.Name, Value = letterType.Id.ToString() };
        }
    }
}

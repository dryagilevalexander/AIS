using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.ProcessViewModels
{
    public class CreateLetterViewModel
    {
        [Required(ErrorMessage = "Не указан номер документа")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Не указана дата документа")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DepartureDate { get; set; }
        [Required(ErrorMessage = "Не указано имя документа")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указано место назначения документа")]
        public string Destination { get; set; }
        [Required(ErrorMessage = "Не указан способ отправки")]
        public int ShippingMethodId { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        [Required(ErrorMessage = "Не указан тип письма")]
        public int LetterTypeId { get; set; }
        public LetterType LetterType { get; set; }
        public IEnumerable<SelectListItem>? ShippingMethods { get; set; }
        public IEnumerable<SelectListItem>? LetterTypes { get; set; }
        public IFormFileCollection? Enclosure { get; set; }
        public IEnumerable<MyFile>? MyFiles { get; set; }

        public async Task Fill(ILetterService _letterService)
        {
            var shippingMethods = await _letterService.GetAllShippingMethods();
            ShippingMethods = from shippingMethod in shippingMethods select new SelectListItem { Text = shippingMethod.Name, Value = shippingMethod.Id.ToString() };
            var letterTypes = await _letterService.GetAllletterTypes();
            LetterTypes = from letterType in letterTypes select new SelectListItem { Text = letterType.Name, Value = letterType.Id.ToString() };
        }
    }
}

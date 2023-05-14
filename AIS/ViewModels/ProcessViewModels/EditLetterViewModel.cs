using AIS.ErrorManager;
using AIS.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AIS.ViewModels.ProcessViewModels
{
    public class EditLetterViewModel
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

        public async Task Fill(int id, ILetterService _letterService)
        {
            Letter? letter = await _letterService.GetLetterById(id);
            if(letter == null) throw new AisException("документ не найдне", HttpStatusCode.BadRequest);
            var shippingMethods = await _letterService.GetAllShippingMethods();
            var letterTypes = await _letterService.GetAllletterTypes();


            Number = letter.Number;
            DepartureDate = letter.DepartureDate;
            Name = letter.Name;
            Destination = letter.Destination;
            ShippingMethodId = letter.ShippingMethodId;
            LetterTypeId = letter.LetterTypeId;

            ShippingMethods = from shippingMethod in shippingMethods select new SelectListItem { Text = shippingMethod.Name, Value = shippingMethod.Id.ToString() };
            LetterTypes = from letterType in letterTypes select new SelectListItem { Text = letterType.Name, Value = letterType.Id.ToString() };
        }
    }
}

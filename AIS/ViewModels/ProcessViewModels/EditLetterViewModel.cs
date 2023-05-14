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

        public async Task Fill(int id, ILetterService _letterService, IEnclosureService _enclosureService)
        {
            Letter? letter = await _letterService.GetLetterById(id);
            if(letter == null) throw new AisException("документ не найдне", HttpStatusCode.BadRequest);
            var shippingMethods = await _letterService.GetAllShippingMethods();
            var letterTypes = await _letterService.GetAllletterTypes();
            IEnumerable<MyFile> enclosures = await _enclosureService.GetMyEnclosuresByLetterId(id);

            Number = letter.Number;
            DepartureDate = letter.DepartureDate;
            Name = letter.Name;
            Destination = letter.Destination;
            ShippingMethodId = letter.ShippingMethodId;
            LetterTypeId = letter.LetterTypeId;
            MyFiles = enclosures;



            ShippingMethods = from shippingMethod in shippingMethods select new SelectListItem { Text = shippingMethod.Name, Value = shippingMethod.Id.ToString() };
            LetterTypes = from letterType in letterTypes select new SelectListItem { Text = letterType.Name, Value = letterType.Id.ToString() };
        }
    }
}

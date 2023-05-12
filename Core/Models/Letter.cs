using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Letter
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
    }
}

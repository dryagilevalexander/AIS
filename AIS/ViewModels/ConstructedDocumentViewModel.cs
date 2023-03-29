using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels
{
    public class ConstructedDocumentViewModel
    {
       public string? NumberOfContract { get; set; }

       [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
       public DateTime? DateStart { get; set; }
       
       [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
       public DateTime? DateEnd { get; set; }
       public decimal Cost { get; set; }
       public string PartnerName { get; set; }
       public string SubjectOfContract { get; set; }
       public string NameInServer { get; set; }
       public string Name { get; set; }
    }


}


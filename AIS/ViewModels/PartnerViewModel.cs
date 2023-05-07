using Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels
{
    public class PartnerViewModel
    {
        public int PartnerTypeId { get; set; }
        public IEnumerable<SelectListItem>? PartnerTypes { get; set; }
    }


}


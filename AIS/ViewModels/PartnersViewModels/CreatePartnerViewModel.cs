using AIS.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AIS.ViewModels.PartnersViewModels
{
    public class СreatePartnerViewModel
    {
        public int PartnerTypeId { get; set; }
        public IEnumerable<SelectListItem>? PartnerTypes { get; set; }
    
    public async Task Fill(IPartnerService _partnerService)
        {
            var partnerTypes = await _partnerService.GetPartnerTypes();
            PartnerTypes = from partnerType in partnerTypes select new SelectListItem { Text = partnerType.Name, Value = partnerType.Id.ToString() };
        }
    }


}


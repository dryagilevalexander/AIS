using AIS.Services;
using Infrastructure.Models;
using System.Runtime.CompilerServices;

namespace AIS.ViewModels.PartnersViewModels
{
    public class PartnersViewModel
    {
        public IEnumerable<PartnerModel> Partners { get; set; }
        public async Task Fill(IPartnerService _partnerService)
        {
            IEnumerable<Partner> partners = await _partnerService.GetPartnersEagerLoading();
            List<PartnerModel> partnersData = new List<PartnerModel>();
            foreach (Partner partner in partners)
            {
                PartnerModel partnerModel = new PartnerModel();

                partnerModel.Id = partner.Id;
                if (partner.PartnerTypeId == 1) partnerModel.Name = partner.Name;
                else partnerModel.Name = partner.Fio;
                partnerModel.Address = partner.Address;
                partnerModel.Email = partner.Email;
                partnerModel.PhoneNumber = partner.PhoneNumber;
                partnerModel.PartnerType = partner.PartnerType;

                partnersData.Add(partnerModel);
            }
            Partners = partnersData.ToList();
        }
    }
    public class PartnerModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public PartnerType PartnerType { get; set; } = null!;
    }
}
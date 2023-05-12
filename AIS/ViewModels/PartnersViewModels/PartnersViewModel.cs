using Infrastructure.Models;
using System.Runtime.CompilerServices;

namespace AIS.ViewModels.PartnersViewModels
{
    public class PartnersViewModel
    {
        public IEnumerable<PartnerModel> Partners { get; set; }
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
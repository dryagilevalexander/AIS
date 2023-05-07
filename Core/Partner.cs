using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace Core
{
    public class Partner
    {
        public int Id { get; set; }
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int PartnerStatusId { get; set; }
        public PartnerStatus PartnerStatus { get; set; } = null!;
        public int PartnerTypeId { get; set; }
        public PartnerType PartnerType { get; set; } = null!;
        public int PartnerCategoryId { get; set; }
        public PartnerCategory PartnerCategory { get; set; } = null!;
        public List<Contract>? Contracts { get; set; }
    }
}
using AIS.ViewModels;
using Core;

namespace AIS.Services
{
    public interface IPartnerService
    {
        Task<IEnumerable<Partner>> GetPartnersEagerLoading();
        Task <IEnumerable<Partner>> GetPartners();
        Task<IEnumerable<Partner>> GetPartnersByPartnerCategoryId(int id);
        Task<Partner?> GetPartner(int id);
        Task<Partner?> GetPartnerEagerLoading(int id);
        Task<bool> CreatePartner(PartnerViewModel partnerViewModel);
        Task<bool> EditPartner(Partner partner);
        Task<bool> DeletePartner(int? id);
        Task<IEnumerable<DirectorType>> GetDirectorTypes();
        Task<IEnumerable<PartnerStatus>> GetPartnerStatuses();
        Task<IEnumerable<PartnerType>> GetPartnerTypes();
        Task<IEnumerable<Partner>> GetPartnersWithoutOurOrganization();
        Task<Partner> GetOurOrganization();
        Task<DirectorType?> GetDirectorTypeById(int? id);
        Task<PartnerType> GetPartnerTypeById(int? id);
    }
}

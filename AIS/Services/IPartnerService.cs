using AIS.ViewModels;
using Core;

namespace AIS.Services
{
    public interface IPartnerService
    {
        Task<IEnumerable<Partner>> GetPartnersEagerLoading();
        Task <IEnumerable<PartnerOrganization>> GetPartnersOrganizations();
        Task<IEnumerable<PartnerOrganization>> GetPartnersByPartnerCategoryId(int id);
        Task<PartnerOrganization?> GetPartner(int id);
        Task<Partner?> GetPartnerEagerLoading(int id);
        Task<bool> CreatePartnerOrganization(PartnerOrganization partner);
        Task<bool> EditPartner(Partner partner);
        Task<bool> DeletePartner(int? id);
        Task<IEnumerable<DirectorType>> GetDirectorTypes();
        Task<IEnumerable<PartnerStatus>> GetPartnerStatuses();
        Task<IEnumerable<PartnerType>> GetPartnerTypes();
        Task<IEnumerable<PartnerOrganization>> GetPartnersWithoutOurOrganization();
        Task<PartnerOrganization> GetOurOrganization();
        Task<DirectorType?> GetDirectorTypeById(int? id);
        Task<PartnerType> GetPartnerTypeById(int? id);
    }
}

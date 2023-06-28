using AIS.ViewModels.PartnersViewModels;
using Infrastructure;
using Infrastructure.Models;

namespace AIS.Services
{
    public interface IPartnerService
    {
        Task<IEnumerable<Partner>> GetPartnersEagerLoading();
        Task <IEnumerable<Partner>> GetPartners();
        Task<IEnumerable<Partner>> GetPartnersByPartnerCategoryId(int id);
        Task<Partner> GetPartner(int id);
        Task<Partner> GetPartnerEagerLoading(int id);
        Task CreatePartner(Partner partner);
        Task CreatePartnerOrganization(CreatePartnerOrganizationViewModel model);
        Task CreatePartnerIp(CreatePartnerIpViewModel model);
        Task CreatePartnerFl(CreatePartnerFlViewModel model);
        Task EditPartnerOrganization(EditPartnerOrganizationViewModel model);
        Task DeletePartner(int? id);
        Task<IEnumerable<DirectorType>> GetDirectorTypes();
        Task<IEnumerable<PartnerStatus>> GetPartnerStatuses();
        Task<IEnumerable<PartnerType>> GetPartnerTypes();
        Task<IEnumerable<Partner>> GetPartnersWithoutOurOrganization();
        Task<DirectorType> GetDirectorTypeById(int? id);
        Task<PartnerType> GetPartnerTypeById(int? id);
        Task<IEnumerable<PartnerCategory>> GetCategories();
        Task EditPartnerIp(EditPartnerIpViewModel model);
        Task EditPartnerFl(EditPartnerFlViewModel model);
        Task<Partner> GetMainOrganization();
        Task<List<Employee>> GetEmployeesByPartnerId(int id);
        Task<Partner> GetPartnerInformation(string inn);
    }
}

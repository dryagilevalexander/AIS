using AIS.ViewModels;
using Core;
using Microsoft.EntityFrameworkCore;


namespace AIS.Services
{
    public class PartnerService : IPartnerService
    {
        private CoreContext db;

        public PartnerService(CoreContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Partner>> GetPartnersEagerLoading()
        {
            try
            {
                IEnumerable<Partner> partners = await db.Partners.Include(u => u.PartnerType).ToListAsync();
                return partners;
            }
            catch
            {
                return null;
            }
        }

        public async Task <IEnumerable<PartnerOrganization>> GetPartnersOrganizations()
        {
            try
            {
                IEnumerable<PartnerOrganization> partners = await db.PartnersOrganizations.ToListAsync();
                return partners;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<PartnerOrganization>> GetPartnersByPartnerCategoryId(int id)
        {
            try
            {
                IEnumerable<PartnerOrganization> partners = await db.PartnersOrganizations.Where(p => p.PartnerCategoryId == id).ToListAsync();
                return partners;
            }
            catch
            {
                return null;
            }
        }

        public async Task<PartnerOrganization?> GetPartner(int id)
        {
            try 
            { 
            PartnerOrganization? partner = await db.PartnersOrganizations.Include(p => p.DirectorType).FirstOrDefaultAsync(p => p.Id == id);
            return partner;
            }
            catch
            {
            return null;
            }
        }

        public async Task<Partner?> GetPartnerEagerLoading(int id)
        {
            try
            {
                Partner? partner = await db.Partners.Include(u => u.PartnerType).FirstOrDefaultAsync(p => p.Id == id);
                return partner;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> CreatePartnerOrganization(PartnerOrganization partner)
        {
            try
            {
                db.PartnersOrganizations.Add(partner);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditPartner(Partner partner)
        {
            try
            {
                db.Partners.Update(partner);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePartner(int? id)
        {
            if (id != null)
            {
                Partner partner = new Partner { Id = id.Value };
                db.Entry(partner).State = EntityState.Deleted;

                List<Employee> employeers = await db.Employeers.ToListAsync();
                foreach (var employee in employeers)
                {
                    if (employee.PartnerOrganization is not null)
                    {
                        if (employee.PartnerOrganization.Id == id)
                        {
                            employee.PartnerOrganization = null;
                            db.Employeers.Update(employee);
                            await db.SaveChangesAsync();
                        }
                    }
                }

                List<Contract> Contracts = await db.Contracts.Where(p=>p.PartnerOrganizationId==id).ToListAsync();
                foreach (var contract in Contracts)
                {
                            db.Contracts.Remove(contract);
                            await db.SaveChangesAsync();
                }

                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<DirectorType>> GetDirectorTypes()
        {
            try
            {
                IEnumerable<DirectorType> directorTypes = await db.DirectorTypes.ToListAsync();
                return directorTypes;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<PartnerStatus>> GetPartnerStatuses()
        {
            try
            {
                IEnumerable<PartnerStatus> partnerStatuses = await db.PartnerStatuses.ToListAsync();
                return partnerStatuses;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<PartnerType>> GetPartnerTypes()
        {
            try
            {
                IEnumerable<PartnerType> partnerTypes = await db.PartnerTypes.ToListAsync();
                return partnerTypes;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<PartnerOrganization>> GetPartnersWithoutOurOrganization()
        {
                return db.PartnersOrganizations.Where(p => p.PartnerStatusId != 1).ToList();
        }


        public async Task<PartnerType> GetPartnerTypeById(int? id)
        {
            return await db.PartnerTypes.FirstOrDefaultAsync(p => p.Id == id.Value);
        }

        public async Task<PartnerOrganization> GetOurOrganization()
        {
            return db.PartnersOrganizations.Include(p => p.DirectorType).FirstOrDefault(p => p.PartnerStatusId == 1);
        }

        public async Task<DirectorType?> GetDirectorTypeById(int? id)
        {
            return await db.DirectorTypes.FirstOrDefaultAsync(p => p.Id == id);
        }

        public PartnerOrganization GetMainOrganization()
        {
            return db.PartnersOrganizations.Include(p => p.DirectorType).FirstOrDefault(p => p.PartnerStatusId == 1);
        }
    }
}

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

        public async Task <IEnumerable<Partner>> GetPartners()
        {
            try
            {
                IEnumerable<Partner> partners = await db.Partners.ToListAsync();
                return partners;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Partner?> GetPartner(int id)
        {
            try 
            { 
            Partner? partner = await db.Partners.Include(p => p.DirectorType).FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task<bool> CreatePartner(PartnerViewModel partnerViewModel)
        {
            try 
            {
                Partner partner = new Partner
                {
                    Name = partnerViewModel.Name,
                    ShortName = partnerViewModel.ShortName,
                    Address = partnerViewModel.Address,
                    Email = partnerViewModel.Email,
                    PhoneNumber = partnerViewModel.PhoneNumber,
                    INN = partnerViewModel.INN,
                    KPP = partnerViewModel.KPP,
                    OGRN = partnerViewModel.OGRN,
                    Bank = partnerViewModel.Bank,
                    Account = partnerViewModel.Account,
                    CorrespondentAccount = partnerViewModel.CorrespondentAccount,
                    BIK = partnerViewModel.BIK,
                    DirectorTypeId = partnerViewModel.DirectorTypeId,
                    DirectorName = partnerViewModel.DirectorName,
                    PartnerStatusId = partnerViewModel.PartnerStatusId,
                    PartnerTypeId = partnerViewModel.PartnerTypeId,
                    PassportSeries = partnerViewModel.PassportSeries,
                    PassportNumber = partnerViewModel.PassportNumber,
                    PassportDateOfIssue = partnerViewModel.PassportDateOfIssue,
                    PassportDateOfBirth = partnerViewModel.PassportDateOfBirth,
                    PassportPlaseOfIssue = partnerViewModel.PassportPlaseOfIssue,
                    PassportDivisionCode = partnerViewModel.PassportDivisionCode
                };

            db.Partners.Add(partner);
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
                    if (employee.Partner is not null)
                    {
                        if (employee.Partner.Id == id)
                        {
                            employee.Partner = null;
                            db.Employeers.Update(employee);
                            await db.SaveChangesAsync();
                        }
                    }
                }

                List<Contract> Contracts = await db.Contracts.Where(p=>p.PartnerId==id).ToListAsync();
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

        public async Task<IEnumerable<Partner>> GetPartnersWithoutOurOrganization()
        {
                return db.Partners.Where(p => p.PartnerStatusId != 1).ToList();
        }


        public async Task<PartnerType> GetPartnerTypeById(int? id)
        {
            return await db.PartnerTypes.FirstOrDefaultAsync(p => p.Id == id.Value);
        }

        public async Task<Partner> GetOurOrganization()
        {
            return db.Partners.FirstOrDefault(p => p.PartnerStatusId == 1);
        }

        public async Task<DirectorType?> GetDirectorTypeById(int? id)
        {
            return await db.DirectorTypes.FirstOrDefaultAsync(p => p.Id == id);
        }

        public Partner GetMainOrganization()
        {
            return db.Partners.Include(p => p.DirectorType).FirstOrDefault(p => p.PartnerStatusId == 1);
        }
    }
}

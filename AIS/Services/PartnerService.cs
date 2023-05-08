using AIS.ViewModels.PartnersViewModels;
using Core;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
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

        public async Task<IEnumerable<Partner>> GetPartnersByPartnerCategoryId(int id)
        {
            try
            {
                IEnumerable<Partner> partners = await db.Partners.Where(p => p.PartnerCategoryId == id).ToListAsync();
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

        public async Task<IEnumerable<PartnerCategory>> GetCategories()
        {
            try
            {
                var categories = await db.PartnerCategories.ToListAsync();
                return categories;
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
        public async Task<bool> CreatePartner(Partner partner)
        {
                db.Partners.Add(partner);
                await db.SaveChangesAsync();
                return true;
         
        }

        public async Task<bool> EditPartnerOrganization(EditPartnerOrganizationViewModel model)
        {
            try
            {
                var partner = await db.Partners.FirstOrDefaultAsync(p => p.Id == model.Id);

                partner.Name = model.Name;
                partner.ShortName = model.ShortName;
                partner.INN = model.INN;
                partner.KPP = model.KPP;
                partner.DirectorTypeId = model.DirectorTypeId;
                partner.DirectorName = model.DirectorName;
                partner.DirectorNameR = model.DirectorNameR;
                partner.Bank = model.Bank;
                partner.Account = model.Account;
                partner.CorrespondentAccount = model.CorrespondentAccount;
                partner.BIK = model.BIK;
                partner.OGRN = model.OGRN;
                partner.PartnerCategoryId = model.PartnerCategoryId;
                partner.PartnerTypeId = model.PartnerTypeId;
                partner.Address = model.Address;
                partner.PhoneNumber = model.PhoneNumber;
                partner.Email = model.Email;

                db.Partners.Update(partner);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditPartnerIp(EditPartnerIpViewModel model)
        {
            try
            {
                var partner = await db.Partners.FirstOrDefaultAsync(p => p.Id == model.Id);
                

                partner.Fio = model.Fio;
                partner.ShortFio = model.ShortFio;
                partner.ShortFioR = model.ShortFioR;
                partner.Address = model.Address;
                partner.Email = model.Email;
                partner.PhoneNumber = model.PhoneNumber;
                partner.INN = model.INN;
                partner.PartnerStatusId = model.PartnerStatusId;
                partner.Bank = model.Bank;
                partner.Account = model.Account;
                partner.CorrespondentAccount = model.CorrespondentAccount;
                partner.BIK = model.BIK;
                partner.PassportSeries = model.PassportSeries;
                partner.PassportNumber = model.PassportNumber;
                partner.PassportDateOfIssue = model.PassportDateOfIssue;
                partner.PassportDateOfBirth = model.PassportDateOfBirth;
                partner.PassportPlaseOfIssue = model.PassportPlaseOfIssue;
                partner.PassportDivisionCode = model.PassportDivisionCode;

                db.Partners.Update(partner);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditPartnerFl(EditPartnerFlViewModel model)
        {
            try
            {
                var partner = await db.Partners.FirstOrDefaultAsync(p => p.Id == model.Id);


                partner.Fio = model.Fio;
                partner.ShortFio = model.ShortFio;
                partner.ShortFioR = model.ShortFioR;
                partner.Address = model.Address;
                partner.Email = model.Email;
                partner.PhoneNumber = model.PhoneNumber;
                partner.INN = model.INN;
                partner.PartnerStatusId = model.PartnerStatusId;
                partner.PassportSeries = model.PassportSeries;
                partner.PassportNumber = model.PassportNumber;
                partner.PassportDateOfIssue = model.PassportDateOfIssue;
                partner.PassportDateOfBirth = model.PassportDateOfBirth;
                partner.PassportPlaseOfIssue = model.PassportPlaseOfIssue;
                partner.PassportDivisionCode = model.PassportDivisionCode;

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
            return db.Partners.Include(p => p.DirectorType).FirstOrDefault(p => p.PartnerStatusId == 1);
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

using AIS.ViewModels.PartnersViewModels;
using Core;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AIS.ErrorManager;


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
                throw new AisException("Список контрагентов не получен", HttpStatusCode.BadRequest);
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
                throw new AisException("Список контрагентов не получен", HttpStatusCode.BadRequest);
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
                throw new AisException("Список контрагентов не получен", HttpStatusCode.BadRequest);
            }
        }

        public async Task<Partner?> GetPartner(int id)
        {
            Partner? partner = await db.Partners.Include(p => p.DirectorType).FirstOrDefaultAsync(p => p.Id == id);
            if(partner == null) throw new AisException("Контрагент не найден", HttpStatusCode.BadRequest);
            return partner;
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
                throw new AisException("Категории контрагентов не найдены", HttpStatusCode.BadRequest);
            }
        }

        public async Task<Partner?> GetPartnerEagerLoading(int id)
        {

            Partner? partner = await db.Partners.Include(u => u.PartnerType).FirstOrDefaultAsync(p => p.Id == id);
            if (partner == null) throw new AisException("Контрагент не найден", HttpStatusCode.BadRequest);
            return partner;
        }
        public async Task CreatePartner(Partner partner)
        {
            try
            {
            db.Partners.Add(partner);
            await db.SaveChangesAsync();
            }
            catch
            {
                throw new AisException("Не удалось создать контрагента", HttpStatusCode.BadRequest);
            }
        }

        public async Task EditPartnerOrganization(EditPartnerOrganizationViewModel model)
        {
                Partner? partner = await db.Partners.FirstOrDefaultAsync(p => p.Id == model.Id);
                if(partner == null) throw new AisException("Контрагент не найден", HttpStatusCode.BadRequest);

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
        }

        public async Task EditPartnerIp(EditPartnerIpViewModel model)
        {
                Partner? partner = await db.Partners.FirstOrDefaultAsync(p => p.Id == model.Id);
                if (partner == null) throw new AisException("Контрагент не найден", HttpStatusCode.BadRequest);

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
        }

        public async Task EditPartnerFl(EditPartnerFlViewModel model)
        {
                Partner? partner = await db.Partners.FirstOrDefaultAsync(p => p.Id == model.Id);
                if (partner == null) throw new AisException("Контрагент не найден", HttpStatusCode.BadRequest);

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
        }

        public async Task DeletePartner(int? id)
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
            } else throw new AisException("Ошибка удаления контрагента", HttpStatusCode.BadRequest);
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
                throw new AisException("Не получен список типов руководителей", HttpStatusCode.BadRequest);
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
                throw new AisException("Не получен список статусов контрагентов", HttpStatusCode.BadRequest);
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
                throw new AisException("Не получен список типов контрагентов", HttpStatusCode.BadRequest);
            }
        }

        public async Task<IEnumerable<Partner>> GetPartnersWithoutOurOrganization()
        {
                return db.Partners.Where(p => p.PartnerStatusId != 1).ToList();
        }


        public async Task<PartnerType> GetPartnerTypeById(int? id)
        {
            PartnerType? partnerType = await db.PartnerTypes.FirstOrDefaultAsync(p => p.Id == id.Value);
            if (partnerType == null) throw new AisException("Не найден тип контрагента", HttpStatusCode.BadRequest);
            return partnerType;
        }

        public async Task<DirectorType?> GetDirectorTypeById(int? id)
        {
            DirectorType? directorType = await db.DirectorTypes.FirstOrDefaultAsync(p => p.Id == id);
            if (directorType == null) throw new AisException("Не найден тип руководителя", HttpStatusCode.BadRequest);
            return directorType;
        }

        public async Task<Partner> GetMainOrganization()
        {
            Partner? partner = await db.Partners.Include(p => p.DirectorType).FirstOrDefaultAsync(p => p.PartnerStatusId == 1);
            if (partner == null) throw new AisException("Головная организация не найдена", HttpStatusCode.BadRequest);
            return partner;
        }
    }
}

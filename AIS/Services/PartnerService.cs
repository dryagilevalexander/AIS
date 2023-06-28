using AIS.ViewModels.PartnersViewModels;
using Infrastructure;
using Infrastructure.Models;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using System.Net;
using AIS.ErrorManager;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AIS.Services
{
    public class PartnerService : IPartnerService
    {
        private AisDbContext db;

        public PartnerService(AisDbContext context)
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

        public async Task<Partner> GetPartner(int id)
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

        public async Task<Partner> GetPartnerEagerLoading(int id)
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

        public async Task CreatePartnerOrganization(CreatePartnerOrganizationViewModel model)
        {
            Partner partner = new Partner()
            {
                Name = model.Name,
                ShortName = model.ShortName,
                INN = model.INN,
                KPP = model.KPP,
                DirectorTypeId = model.DirectorTypeId,
                DirectorName = model.DirectorName,
                DirectorNameR = model.DirectorNameR,
                Bank = model.Bank,
                Account = model.Account,
                CorrespondentAccount = model.CorrespondentAccount,
                BIK = model.BIK,
                OGRN = model.OGRN,
                PartnerCategoryId = model.PartnerCategoryId,
                PartnerTypeId = model.PartnerTypeId,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };

            await CreatePartner(partner);
        }

        public async Task CreatePartnerIp(CreatePartnerIpViewModel model)
        {
            Partner partner = new Partner()
            {
                PartnerTypeId = model.PartnerTypeId,
                Fio = model.Fio,
                ShortFio = model.ShortFio,
                ShortFioR = model.ShortFioR,
                Address = model.Address,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                INN = model.INN,
                PartnerStatusId = model.PartnerStatusId,
                Bank = model.Bank,
                Account = model.Account,
                CorrespondentAccount = model.CorrespondentAccount,
                BIK = model.BIK,
                PassportSeries = model.PassportSeries,
                PassportNumber = model.PassportNumber,
                PassportDateOfIssue = model.PassportDateOfIssue,
                PassportDateOfBirth = model.PassportDateOfBirth,
                PassportPlaseOfIssue = model.PassportPlaseOfIssue,
                PassportDivisionCode = model.PassportDivisionCode
            };

            await CreatePartner(partner);
        }

        public async Task CreatePartnerFl(CreatePartnerFlViewModel model)
        {
            Partner partner = new Partner()
            {
                PartnerTypeId = model.PartnerTypeId,
                Fio = model.Fio,
                ShortFio = model.ShortFio,
                ShortFioR = model.ShortFioR,
                Address = model.Address,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                INN = model.INN,
                PartnerStatusId = model.PartnerStatusId,
                PassportSeries = model.PassportSeries,
                PassportNumber = model.PassportNumber,
                PassportDateOfIssue = model.PassportDateOfIssue,
                PassportDateOfBirth = model.PassportDateOfBirth,
                PassportPlaseOfIssue = model.PassportPlaseOfIssue,
                PassportDivisionCode = model.PassportDivisionCode
            };

            await CreatePartner(partner);
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

        public async Task<DirectorType> GetDirectorTypeById(int? id)
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

        public async Task<List<Employee>> GetEmployeesByPartnerId(int id)
        {
            return await db.Employeers.Where(p => p.PartnerId == id).ToListAsync();
        }

        public async Task<Partner?> GetPartnerInformation(string inn)
        {
            HttpClient httpClient = new HttpClient();

            var serverAddress = "https://egrul.nalog.ru/index.html";
            using var response = await httpClient.GetAsync(serverAddress);

            //Получаем куки с ключом из заголовков
            var dateValues = response.Headers.GetValues("Set-Cookie");
            string key = (dateValues.FirstOrDefault()).Split(';')[0];



            //Формируем тело запроса к серверу
            ReqBody reqBody = new ReqBody
            {
                vyp3CaptchaToken = "",
                page = "",
                query = inn,
                region = "",
                PreventChromeAutocomplete = ""
            };

            //Создаем тело запроса
            JsonContent content = JsonContent.Create(reqBody);
            // передаем ключ в заголовок
            content.Headers.Add("Cookie", key);

            //Отправляем запрос
            var resp = await httpClient.PostAsync("https://egrul.nalog.ru/", content);
            response.EnsureSuccessStatusCode();

            //Получаем частичную ссылку на искомую организацию
            string responseBody = await resp.Content.ReadAsStringAsync();

            //Получаем полную ссылку
            string sURL;
            sURL = "https://egrul.nalog.ru/search-result/" + ((responseBody.Split(':')[1]).Split(",")[0]).Replace("\"", "");

            //Отправляем Get запрос к серверу
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            //Получаем ответ
            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string partnerInformation = "";
            partnerInformation = objReader.ReadLine();

            if (partnerInformation != "{\"rows\":[]}") //Если организация найдена
            {
                //Удаляем ненужные символы для приведения строки к json формату
                partnerInformation = partnerInformation.Replace("{\"rows\":", "");
                partnerInformation = partnerInformation.Replace("]}", "]");

                //Преобразуем строку в json
                List<JsonPartnerInformation> jsonPartnerInformation = JsonConvert.DeserializeObject<List<JsonPartnerInformation>>(partnerInformation);


                string directorInformation = (jsonPartnerInformation[0].g).Split(",")[0];
                string directorTypeName = directorInformation.Split(":")[0];
                string directorName = directorInformation.Split(":")[1].Trim();
                int directorType = 1;

                switch (directorTypeName)
                {
                    case "ДИРЕКТОР":
                        directorType = 1;
                        break;
                    case "ГЕНЕРАЛЬНЫЙ ДИРЕКТОР":
                        directorType = 2;
                        break;
                    case "ГЛАВА":
                        directorType = 3;
                        break;
                    case "ЗАВЕДУЮЩИЙ":
                        directorType = 4;
                        break;
                }


                Partner partner = new Partner
                {
                    Address = jsonPartnerInformation[0].a,
                    Name = jsonPartnerInformation[0].n,
                    ShortName = jsonPartnerInformation[0].c,
                    KPP = jsonPartnerInformation[0].p,
                    OGRN = jsonPartnerInformation[0].o,
                    DirectorTypeId = directorType,
                    DirectorName = directorName
                };

                return partner;
            }
            else return null;
    }

        class ReqBody
        {
            public string vyp3CaptchaToken { get; set; }
            public string page { get; set; }
            public string query { get; set; }
            public string nameEq { get; set; }
            public string region { get; set; }
            public string PreventChromeAutocomplete { get; set; }
        }

        public class JsonPartnerInformation
        {
            public string a { get; set; }
            public string c { get; set; }
            public string g { get; set; }
            public string cnt { get; set; }
            public string i { get; set; }
            public string k { get; set; }
            public string o184 { get; set; }
            public string n { get; set; }
            public string o { get; set; }
            public string p { get; set; }
            public string r { get; set; }
            public string t { get; set; }
            public string pg { get; set; }
            public string tot { get; set; }
        }
    }
}

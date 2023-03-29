using AIS.ViewModels;
using Core;
using Microsoft.EntityFrameworkCore;

namespace AIS.Services
{
    public class TemplatesService: ITemplatesService
    {
        private CoreContext db;
        private IPartnerService _partnerService;
        private IContractsService _contractsService;
        IWebHostEnvironment _appEnvironment;

        public TemplatesService(CoreContext coreContext, IPartnerService partnerService, IContractsService contractsService, IWebHostEnvironment appEnvironment)
        {
            db = coreContext;
            _partnerService = partnerService;
            _contractsService = contractsService;
            _appEnvironment = appEnvironment;
        }

        public async Task<DocumentTemplate> GetDocumentTemplateWithFilesById(int id)
        {
            return await db.DocumentTemplates.Include(r => r.TemplateFile).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<DocumentTemplate>> GetAllTemplatesWitnToFAndToS()
        {
            return await db.DocumentTemplates.Include(u => u.TypeOfContract).Include(u => u.TypeOfStateRegs).ToListAsync();
        }

        public async Task<IEnumerable<DocumentTemplate>> GetTemplatesWithTypeOfContractAndPartnerType(int typeOfContractId, int partnerTypeId)
        {
            return await db.DocumentTemplates.Include(u => u.TypeOfContract).Include(u => u.PartnerTypes).Where(p => p.TypeOfContractId == typeOfContractId).Where(p => p.PartnerTypes.Any(p => p.Id == partnerTypeId)).ToListAsync();
        }

        public async Task<DocumentTemplate> GetTemplateByIdEagerLoading(int id)
        {
            return await db.DocumentTemplates.Include(r => r.TemplateFile).Include(r => r.TypeOfStateRegs).Include(r => r.PartnerTypes).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> DeleteTemplate(int? id)
        {
            try
            {
                if (id != null)
                {
                    DocumentTemplate docTemplate = new DocumentTemplate();
                    docTemplate = await GetTemplateByIdEagerLoading(id.Value);

                    db.DocumentTemplates.Remove(docTemplate);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> CreateTemplate(TemplateViewModel tvm)
        {
            try
            {
                MyFile myFile = new MyFile();
                List<int> typesOfStateRegIds = new List<int>();
                List<TypeOfStateReg> typesOfStateRegs = new List<TypeOfStateReg>();
                List<PartnerType> partnerTypes = new List<PartnerType>();
                DocumentTemplate docTemplate = new DocumentTemplate
                {
                    TypeOfContractId = tvm.TypeOfContractId,
                    NameOfTemplate = tvm.NameOfTemplate,
                    NameOutput = tvm.OutputName
                };

                foreach (var typeOfStateRegId in tvm.TypeOfStateRegIds)
                {
                    typesOfStateRegs.Add(await _contractsService.GetTypeOfStateRegById(typeOfStateRegId));

                }

                foreach (var partnerTypeId in tvm.PartnerTypeIds)
                {
                    partnerTypes.Add(await _partnerService.GetPartnerTypeById(partnerTypeId));

                }

                docTemplate.PartnerTypes = partnerTypes;
                docTemplate.TypeOfStateRegs = typesOfStateRegs;


                if (tvm.Enclosure is not null)
                {
                    var ext = Path.GetExtension(tvm.Enclosure.FileName);
                    string fileName = String.Format(@"{0}" + ext, System.Guid.NewGuid());
                    string path = "\\Files\\Templates\\";
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path + fileName, FileMode.Create))
                    {
                        await tvm.Enclosure.CopyToAsync(fileStream);
                    }


                    myFile.Name = tvm.Enclosure.FileName;
                    myFile.NameInServer = fileName;
                    myFile.FilePath = _appEnvironment.WebRootPath + path;
                }

                docTemplate.TemplateFile = myFile;
                db.DocumentTemplates.Update(docTemplate);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}

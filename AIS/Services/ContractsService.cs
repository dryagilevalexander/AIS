using AIS.ViewModels;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AIS.Services
{
    public class ContractsService: IContractsService
    {
        private CoreContext db;
        IWebHostEnvironment _appEnvironment;
        public ContractsService(CoreContext coreContext, IWebHostEnvironment appEnvironment)
        {
            db = coreContext;
            _appEnvironment = appEnvironment;
        }
        public async Task<List<Contract>> GetActiveContractsEagerLoading()
        {
            return await db.Contracts
                           .Include(u => u.Partner)
                           .Include(u => u.MyContractStatus)
                           .Where(p => p.MyContractStatusId != 6)
                           .ToListAsync();
        }

        public async Task<List<Contract>> GetArchiveContractsEagerLoading()
        {
            return await db.Contracts
                           .Include(u => u.Partner)
                           .Include(u => u.MyContractStatus)
                           .Where(p => p.MyContractStatusId == 6)
                           .ToListAsync();
        }

        public async Task<IEnumerable<TypeOfContract>> GetTypeOfContracts()
        { 
            return await db.TypeOfContracts.ToListAsync();
        }

        public async Task<IEnumerable<TypeOfStateReg>> GetTypeOfStateRegs()
        {
            return await db.TypeOfStateRegs.ToListAsync();
        }
        public async Task<TypeOfStateReg> GetTypeOfStateRegById(int id)
        {
            return await db.TypeOfStateRegs.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ArticleOfLaw>> GetArticleOfLaws()
        {
            return await db.ArticleOfLaws.ToListAsync();
        }

        public async Task<IEnumerable<MyContractStatus>> GetMyContractStatuses()
        {
            return await db.MyContractStatuses.ToListAsync();
        }

        public async Task<bool> CreateContract(MyContractViewModel mcvm)
        {
            try
            {
                List<string> enclosure = new List<string>();
                List<MyFile> myFiles = new List<MyFile>();

                Contract myContract = new Contract
                {
                    TypeOfContract = mcvm.TypeOfContractId,
                    NumberOfContract = mcvm.NumberOfContract,
                    MyContractStatusId = mcvm.MyContractStatusId,
                    DateStart = mcvm.DateStart,
                    DateEnd = mcvm.DateEnd,
                    PartnerId = mcvm.PartnerId,
                    SubjectOfContract = mcvm.SubjectOfContract,
                    Cost = mcvm.Cost
                };

                if (mcvm.Enclosure is not null)
                {
                    foreach (var uploadedFile in mcvm.Enclosure)
                    {
                        var ext = Path.GetExtension(uploadedFile.FileName);
                        string fileName = String.Format(@"{0}" + ext, System.Guid.NewGuid());
                        string path = "/Files/";
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path + fileName, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }

                        MyFile myFile = new MyFile
                        {
                            Name = uploadedFile.FileName,
                            NameInServer = fileName,
                            FilePath = _appEnvironment.WebRootPath + path
                        };

                        myFiles.Add(myFile);
                    }
                }
                myContract.MyFiles = myFiles;
                db.Contracts.Update(myContract);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EditContract(MyContractViewModel mcvm)
        {
            try
            {
                List<string> enclosure = new List<string>();
                List<MyFile> myFiles = new List<MyFile>();

                Contract? contract = await db.Contracts.Include(u => u.MyFiles).FirstOrDefaultAsync(p => p.Id == mcvm.Id);

                contract.NumberOfContract = mcvm.NumberOfContract;
                contract.DateStart = mcvm.DateStart;
                contract.DateEnd = mcvm.DateEnd;
                contract.PartnerId = mcvm.PartnerId;
                contract.MyContractStatusId = mcvm.MyContractStatusId;
                contract.SubjectOfContract = mcvm.SubjectOfContract;
                contract.Cost = mcvm.Cost;

                if (mcvm.Enclosure is not null)
                {
                    foreach (var uploadedFile in mcvm.Enclosure)
                    {
                        var ext = Path.GetExtension(uploadedFile.FileName);
                        string fileName = String.Format(@"{0}" + ext, System.Guid.NewGuid());
                        string path = "/Files/";
                        // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path + fileName, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }

                        MyFile myFile = new MyFile
                        {
                            Name = uploadedFile.FileName,
                            NameInServer = fileName,
                            FilePath = _appEnvironment.WebRootPath + path
                        };

                        myFiles.Add(myFile);
                    }
                }
                contract.MyFiles = myFiles;
                db.Contracts.Update(contract);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteContract(int? id)
        {
            try
            {
                if (id != null)
                {
                    Contract myContract = db.Contracts.Include(r => r.MyFiles).FirstOrDefault(p => p.Id == id.Value);
                    List<MyFile> myFiles = myContract.MyFiles;
                    foreach (var myFile in myFiles)
                    {
                        db.MyFiles.Remove(myFile);
                        await db.SaveChangesAsync();
                    }
                    db.Contracts.Remove(myContract);
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

        public async Task<Contract> GetContractByIdWithMyFiles(int id)
        {
            return await db.Contracts.Include(u => u.MyFiles).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
